using System.Security.Claims;
using Blazored.Toast.Services;
using EMS.Model.DTOs;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.RotatingRequest.Update
{
    public partial class UpdateOwnRequest
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
        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }
        public UserModel User { get; set; } = new UserModel();
        private List<DepartmentModel> Departments { get; set; } = new();
        private List<EquipmentTypeModel> EquipmentTypes { get; set; } = new();


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadUserFromToken();
            var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/RotatingRequest/{ID}");
            if (res != null && res.Success)
            {
                Model = JsonConvert.DeserializeObject<RotatingRequestModel>(res.Data.ToString());
            }
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
            var res = await ApiClient.PutAsync<BaseResponseModel, RotatingRequestModel>($"/api/RotatingRequest/{ID}", Model);
            if (res != null && res.Success)
            {
                ToastService.ShowSuccess("Cập nhật yêu cầu thành công!");
                NavigationManager.NavigateTo("/rotating/ownrequest");
            }
        }
    }
}
