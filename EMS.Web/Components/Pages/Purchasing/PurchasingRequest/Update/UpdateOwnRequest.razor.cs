using Blazored.Toast.Services;
using EMS.Model.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using EMS.Model.Models.Others;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EMS.Web.Components.Pages.Purchasing.PurchasingRequest.Update
{
	public partial class UpdateOwnRequest
	{
		[Parameter]
		public int ID { get; set; }
		public PurchasingRequestModel Model { get; set; } = new PurchasingRequestModel
		{
			PurchasingRequestDetails = new List<PurchasingRequestDetailModel>()
		};
		private List<EquipmentTypeModel> EquipmentTypes { get; set; } = new();

		[Inject]
		private ApiClient ApiClient { get; set; }
		[Inject]
		private IToastService ToastService { get; set; }
		[Inject]
		private NavigationManager NavigationManager { get; set; }
		[Inject]
		public AuthenticationStateProvider AuthStateProvider { get; set; }
		public UserModel User { get; set; } = new UserModel();

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			await LoadUserFromToken();
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/PurchasingRequest/{ID}");
			if (res != null && res.Success)
			{
				Model = JsonConvert.DeserializeObject<PurchasingRequestModel>(res.Data.ToString());
			}
			if (Model.AcceptanceLv2Status != null)
			{
				ToastService.ShowError("Yêu cầu của bạn đang được duyệt nên không thể cập nhật");
				NavigationManager.NavigateTo("/purchasing/ownrequest");
			}
			var equipmentTypesRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/EquipmentType");
			if (equipmentTypesRes != null && res.Success)
			{
				EquipmentTypes = JsonConvert.DeserializeObject<List<EquipmentTypeModel>>(equipmentTypesRes.Data.ToString());
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

		private void AddDetail()
		{
			Model.PurchasingRequestDetails.Add(new PurchasingRequestDetailModel());
		}

		private void RemoveDetail(PurchasingRequestDetailModel detail)
		{
			Model.PurchasingRequestDetails.Remove(detail);
		}
		public async Task Submit()
		{
			Model.UserId = User.ID;
			var res = await ApiClient.PutAsync<BaseResponseModel, PurchasingRequestModel>($"/api/PurchasingRequest/{ID}", Model);
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Cập nhật yêu cầu thành công!");
				NavigationManager.NavigateTo("/purchasing/ownrequest");
			}
		}
	}
}
