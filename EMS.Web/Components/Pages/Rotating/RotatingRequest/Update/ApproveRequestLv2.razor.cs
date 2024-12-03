using System.Security.Claims;
using Blazored.Toast.Services;
using EMS.Model.DTOs;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Rotating.RotatingRequest.Update
{
    public partial class ApproveRequestLv2
    {
        [Parameter]
        public int ID { get; set; }
        public ApproveRequestDto Model { get; set; } = new();

        [Inject]
        private ApiClient ApiClient { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        public UserModel User { get; set; } = new UserModel();
        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadUserFromToken();
            Model.RequestId = ID;
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
            Model.ReviewerId = User.ID;
			if (string.IsNullOrWhiteSpace(Model.Reason))
			{
				ToastService.ShowError("Vui lòng nhập lý do duyệt!");
				return;
			}
			var res = await ApiClient.PostAsync<BaseResponseModel, ApproveRequestDto>($"/api/RotatingRequest/lv2/approve", Model);
            if (res != null && res.Success)
            {
                ToastService.ShowSuccess("Duyệt yêu cầu thành công!");
                NavigationManager.NavigateTo("/rotating/requestlv1");
            }
        }
    }
}
