using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Web.Components.BaseComponents;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using EMS.Model.Models.Others;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Purchasing.PurchasingRequest.Index
{
	public partial class IndexOwnRequest
	{
		[Inject]
		public ApiClient ApiClient { get; set; }
		public List<PurchasingRequestModel> OwnPurchasingRequestModels { get; set; }
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
			await LoadPurchasingRequest();
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

		protected async Task LoadPurchasingRequest()
		{
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/PurchasingRequest/own/{User.ID}");
			if (res != null && res.Success)
			{
				OwnPurchasingRequestModels = JsonConvert.DeserializeObject<List<PurchasingRequestModel>>(res.Data.ToString());
			}
		}

		protected async Task HandleDelete()
		{
			var requestToDelete = OwnPurchasingRequestModels.FirstOrDefault(r => r.ID == DeleteID);

			// Kiểm tra nếu yêu cầu tồn tại và có trạng thái duyệt cấp 2
			if (requestToDelete != null && requestToDelete.AcceptanceLv2Status != null)
			{
				ToastService.ShowError("Không thể xoá yêu cầu đang đợi duyệt!");
				Modal.Close();
				return;
			}
			var res = await ApiClient.DeleteAsync<BaseResponseModel>($"/api/PurchasingRequest/{DeleteID}");
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Xoá yêu cầu hoàn tất");
				await LoadPurchasingRequest();
				Modal.Close();
			}
		}
	}
}
