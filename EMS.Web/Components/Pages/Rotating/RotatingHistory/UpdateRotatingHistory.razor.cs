using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Rotating.RotatingHistory
{
    public partial class UpdateRotatingHistory
    {
        [Parameter]
        public int ID { get; set; }
        public RotatingHistoryModel Model { get; set; } = new();

        [Inject]
        private ApiClient ApiClient { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        public List<UserModel> RequestUser { get; set; } = new();
        public List<UserModel> ReviewerLv2 { get; set; } = new();
        public List<UserModel> ReviewerLv3 { get; set; } = new();
        private List<DepartmentModel> FromDepartments { get; set; } = new();
        private List<DepartmentModel> ToDepartments { get; set; } = new();
        private List<EquipmentModel> Equipments { get; set; } = new();


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/RotatingHistory/{ID}");
            if (res != null && res.Success)
            {
                Model = JsonConvert.DeserializeObject<RotatingHistoryModel>(res.Data.ToString());
            }

            // Lấy danh sách Departments cho cả FromDepartments và ToDepartments
            var FromDepartmentsRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/Department");
            if (FromDepartmentsRes != null && FromDepartmentsRes.Success)
            {
                FromDepartments = JsonConvert.DeserializeObject<List<DepartmentModel>>(FromDepartmentsRes.Data.ToString());
                ToDepartments = JsonConvert.DeserializeObject<List<DepartmentModel>>(FromDepartmentsRes.Data.ToString());
            }

            // Lấy danh sách Equipments
            var EquipmentsRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/Equipment");
            if (EquipmentsRes != null && EquipmentsRes.Success)
            {
                Equipments = JsonConvert.DeserializeObject<List<EquipmentModel>>(EquipmentsRes.Data.ToString());
            }

            // Lấy danh sách Users cho RequestUser, ReviewerLv2 và ReviewerLv3
            var RequestUserRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/User");
            if (RequestUserRes != null && RequestUserRes.Success)
            {
                var users = JsonConvert.DeserializeObject<List<UserModel>>(RequestUserRes.Data.ToString());
                RequestUser = users;
                ReviewerLv2 = users;
                ReviewerLv3 = users;
            }
        }

        
        public async Task Submit()
        {
            var res = await ApiClient.PutAsync<BaseResponseModel, RotatingHistoryModel>($"/api/RotatingHistory/{ID}", Model);
            if (res != null && res.Success)
            {
                ToastService.ShowSuccess("Cập nhật yêu cầu thành công!");
                NavigationManager.NavigateTo("/rotating/history");
            }
        }
    }
}
