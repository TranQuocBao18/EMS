using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using EMS.Web.Components.BaseComponents;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EMS.Web.Components.Pages.Liquidation.LiquidationRequest.Index
{
	public partial class IndexOwnRequest
	{
		[Inject]
		public ApiClient ApiClient { get; set; }
		public List<LiquidationRequestModel> OwnLiquidationRequestModels { get; set; }
		[Inject]
		public IToastService ToastService { get; set; }
		public AppModal Modal { get; set; }
		public int DeleteID { get; set; }
		public UserModel User { get; set; } = new UserModel();
		[Inject]
		public AuthenticationStateProvider AuthStateProvider { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			await LoadUserFromToken();
			await LoadLiquidationRequest();
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

		protected async Task LoadLiquidationRequest()
		{
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/LiquidationRequest/own/{User.ID}");
			if (res != null && res.Success)
			{
				OwnLiquidationRequestModels = JsonConvert.DeserializeObject<List<LiquidationRequestModel>>(res.Data.ToString());
			}
		}

		protected async Task HandleDelete()
		{
			var res = await ApiClient.DeleteAsync<BaseResponseModel>($"/api/LiquidationRequest/{DeleteID}");
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Xoá yêu cầu hoàn tất");
				await LoadLiquidationRequest();
				Modal.Close();
			}
		}
	}
}
