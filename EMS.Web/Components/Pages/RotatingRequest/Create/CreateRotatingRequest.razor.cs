using System.Security.Claims;
using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.RotatingRequest.Create
{
    public partial class CreateRotatingRequest
    {
        public RotatingRequestModel Model { get; set; } = new();

        [Inject]
        private ApiClient ApiClient { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        private List<DepartmentModel> Departments { get; set; } = new();
        private List<EquipmentTypeModel> EquipmentTypes { get; set; } = new();
        public UserModel User { get; set; } = new UserModel();
        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadUserFromToken();

            var DepartmentsRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/Department");
            if (DepartmentsRes != null && DepartmentsRes.Success)
            {
                Departments = JsonConvert.DeserializeObject<List<DepartmentModel>>(DepartmentsRes.Data.ToString());
            }

            var EquipmentTypesRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/EquipmentType");
            if (EquipmentTypesRes != null && EquipmentTypesRes.Success)
            {
                EquipmentTypes = JsonConvert.DeserializeObject<List<EquipmentTypeModel>>(EquipmentTypesRes.Data.ToString());
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
