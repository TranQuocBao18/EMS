using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using EMS.Model.Models.User;
using EMS.Web.Authentication;
using EMS.Web.Components.BaseComponents;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.User
{
    public partial class RegisterUser
    {
        private RegisterModel registerModel = new RegisterModel();
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        public ApiClient ApiClient { get; set; }
        public AppModal Modal { get; set; }
        public List<RoleModelBlazor> AllRoles { get; set; } = new List<RoleModelBlazor>();
        private string passwordInputType = "password";
        private string passwordIcon = "fas fa-eye"; // Mặc định là biểu tượng mắt
        private string confirmPasswordInputType = "password";
        private string confirmPasswordIcon = "fas fa-eye"; // Mặc định là biểu tượng mắt

        private void TogglePasswordVisibility()
        {
            if (passwordInputType == "password")
            {
                passwordInputType = "text";
                passwordIcon = "fas fa-eye-slash"; // Đổi sang biểu tượng mắt đóng
            }
            else
            {
                passwordInputType = "password";
                passwordIcon = "fas fa-eye"; // Đổi lại biểu tượng mắt mở
            }
        }
        private void ToggleConfirmPasswordVisibility()
        {
            if (confirmPasswordInputType == "password")
            {
                confirmPasswordInputType = "text";
                confirmPasswordIcon = "fas fa-eye-slash"; // Đổi sang biểu tượng mắt đóng
            }
            else
            {
                confirmPasswordInputType = "password";
                confirmPasswordIcon = "fas fa-eye"; // Đổi lại biểu tượng mắt mở
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var rolesRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/Role");
            if (rolesRes != null && rolesRes.Success)
            {
                // Chuyển đổi sang RoleModelBlazor
                var roles = JsonConvert.DeserializeObject<List<RoleModel>>(rolesRes.Data.ToString());
                AllRoles = roles.Select(role => new RoleModelBlazor { ID = role.ID, RoleName = role.RoleName }).ToList();

            }
        }

        private async Task HandleRegister()
        {
            registerModel.RoleIDs = AllRoles
            .Where(role => role.IsSelected)
            .Select(role => role.ID)
            .ToList();
            try
            {
                var res = await ApiClient.PostAsync<BaseResponseModel, RegisterModel>("/api/auth/register", registerModel);
                if (res != null && res.Success)
                {
                    // Hiển thị thông báo thành công
                    ToastService.ShowSuccess("Đăng ký người dùng thành công.");

                    // Có thể thực hiện điều hướng hoặc reset form nếu cần thiết
                    Modal.Open();
                    registerModel = new RegisterModel(); // Reset lại form nếu cần
                }
                else
                {
                    // Nếu API trả về lỗi, hiển thị thông báo lỗi
                    ToastService.ShowError(res?.ErrorMessage ?? "Có lỗi xảy ra.");
                }
            }
            catch (ApiException ex)
            {
                ToastService.ShowError(ex.Message); // Hiển thị thông báo lỗi từ API
            }
        }

        protected async Task HandleLogout()
        {
            Navigation.NavigateTo("/logout");
        }
    }
}
