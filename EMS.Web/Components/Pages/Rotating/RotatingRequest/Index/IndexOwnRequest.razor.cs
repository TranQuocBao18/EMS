using Blazored.Toast.Services;
using System.Security.Claims;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using EMS.Web.Components.BaseComponents;

namespace EMS.Web.Components.Pages.Rotating.RotatingRequest.Index
{
	public partial class IndexOwnRequest
	{
		[Inject]
		public ApiClient ApiClient { get; set; }
		public List<RotatingRequestModel> OwnRotatingRequestModels { get; set; }
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
			await LoadRotatingRequest();
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

		protected async Task LoadRotatingRequest()
		{
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/RotatingRequest/own/{User.ID}");
			if (res != null && res.Success)
			{
				OwnRotatingRequestModels = JsonConvert.DeserializeObject<List<RotatingRequestModel>>(res.Data.ToString());
			}
		}

		protected async Task HandleDelete()
		{
			var res = await ApiClient.DeleteAsync<BaseResponseModel>($"/api/RotatingRequest/{DeleteID}");
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Xoá yêu cầu hoàn tất");
				await LoadRotatingRequest();
				Modal.Close();
			}
		}
	}
}
