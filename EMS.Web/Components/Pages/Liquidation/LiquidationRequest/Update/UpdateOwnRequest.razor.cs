using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EMS.Web.Components.Pages.Liquidation.LiquidationRequest.Update
{
	public partial class UpdateOwnRequest
	{
		[Parameter]
		public int ID { get; set; }
		public LiquidationRequestModel Model { get; set; } = new();

		[Inject]
		private ApiClient ApiClient { get; set; }
		[Inject]
		private IToastService ToastService { get; set; }
		[Inject]
		private NavigationManager NavigationManager { get; set; }
		[Inject]
		public AuthenticationStateProvider AuthStateProvider { get; set; }
		public UserModel User { get; set; } = new UserModel();
		private List<EquipmentModel> Equipments { get; set; } = new();


		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			await LoadUserFromToken();
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/LiquidationRequest/{ID}");
			if (res != null && res.Success)
			{
				Model = JsonConvert.DeserializeObject<LiquidationRequestModel>(res.Data.ToString());
			}
			if (Model.AcceptanceLv2Status != null)
			{
				ToastService.ShowError("Yêu cầu của bạn đang được duyệt nên không thể cập nhật");
				await Task.Delay(2000);
				NavigationManager.NavigateTo("/liquidation/ownrequest");
			}

			var EquipmentsRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/Equipment");
			if (EquipmentsRes != null && EquipmentsRes.Success)
			{
				Equipments = JsonConvert.DeserializeObject<List<EquipmentModel>>(EquipmentsRes.Data.ToString());
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
			var res = await ApiClient.PutAsync<BaseResponseModel, LiquidationRequestModel>($"/api/LiquidationRequest/{ID}", Model);
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Cập nhật yêu cầu thành công!");
				NavigationManager.NavigateTo("/liquidation/ownrequest");
			}
		}
	}
}
