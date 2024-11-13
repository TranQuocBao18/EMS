using System.Security.Claims;
using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Rotating.RotatingRequest.Create
{
	public partial class CreateRotatingRequest
	{
		[Parameter]
		public int ID { get; set; }
		public RotatingRequestModel Model { get; set; } = new();

		[Inject]
		private ApiClient ApiClient { get; set; }
		[Inject]
		private IToastService ToastService { get; set; }
		[Inject]
		private NavigationManager NavigationManager { get; set; }
		private DepartmentModel FromDepartment { get; set; } = new();
		private List<DepartmentModel> ToDepartments { get; set; } = new();
		private EquipmentModel Equipment { get; set; } = new();
		public UserModel User { get; set; } = new UserModel();
		[Inject]
		public AuthenticationStateProvider AuthStateProvider { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			await LoadUserFromToken();

			var EquipmentsRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/Equipment/{ID}");
			if (EquipmentsRes != null && EquipmentsRes.Success)
			{
				Equipment = JsonConvert.DeserializeObject<EquipmentModel>(EquipmentsRes.Data.ToString());
				if (Equipment != null)
				{
					// Gán giá trị cho Model.EquipmentId và Model.FromDepartmentId
					Model.EquipmentId = Equipment.ID;
					Model.FromDepartmentId = Equipment.DepartmentId;
				}
			}
			var FromDepartmentRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/Department/{Equipment.DepartmentId}");
			if (FromDepartmentRes != null && FromDepartmentRes.Success)
			{
				FromDepartment = JsonConvert.DeserializeObject<DepartmentModel>(FromDepartmentRes.Data.ToString());

			}
			var DepartmentsRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/Department");
			if (DepartmentsRes != null && DepartmentsRes.Success)
			{
				ToDepartments = JsonConvert.DeserializeObject<List<DepartmentModel>>(DepartmentsRes.Data.ToString());
			}



		}

		protected async Task LoadUserFromToken()
		{
			var authState = await AuthStateProvider.GetAuthenticationStateAsync();
			var userClaims = authState.User;

			if (userClaims.Identity.IsAuthenticated)
			{
				User.Username = userClaims.FindFirst(ClaimTypes.Name)?.Value;
				// Nếu bạn có thêm các thông tin khác như ID, email, role, thì trích xuất từ claims:
				User.ID = int.Parse(userClaims.FindFirst("UserId")?.Value); // Giả sử bạn lưu UserId trong claim
			}
			else
			{
				ToastService.ShowError("Không thể lấy thông tin người dùng.");
			}
		}

		public async Task Submit()
		{
			Model.UserId = User.ID;
			var res = await ApiClient.PostAsync<BaseResponseModel, RotatingRequestModel>("/api/RotatingRequest/create", Model);
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Tạo yêu cầu thành công!");
				NavigationManager.NavigateTo("/rotating/ownrequest");
			}
		}
	}
}
