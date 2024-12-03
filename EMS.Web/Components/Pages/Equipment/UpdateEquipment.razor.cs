using Blazored.Toast.Services;
using EMS.Model.DTOs;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Equipment
{
	public partial class UpdateEquipment
	{
        [Parameter]
        public int ID { get; set; }
        public EquipmentModel Model { get; set; } = new EquipmentModel();
        public List<EquipmentTypeModel> EquipmentTypes { get; set; } = new();
        public List<UserModel> Users { get; set; } = new();

        [Inject]
        private ApiClient ApiClient { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            

            var equipmentTypeRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/EquipmentType");
            if (equipmentTypeRes != null && equipmentTypeRes.Success)
            {
                EquipmentTypes = JsonConvert.DeserializeObject<List<EquipmentTypeModel>>(equipmentTypeRes.Data.ToString());
            }

            var userRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/User");
            if (userRes != null && userRes.Success)
            {
                Users = JsonConvert.DeserializeObject<List<UserModel>>(userRes.Data.ToString());
            }
            var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/Equipment/{ID}");
            if (res != null && res.Success)
            {
                Model = JsonConvert.DeserializeObject<EquipmentModel>(res.Data.ToString());
            }
        }

        public async Task Submit()
        {
            var res = await ApiClient.PutAsync<BaseResponseModel, EquipmentModel>($"/api/Equipment/{ID}", Model);
            if (res != null && res.Success)
            {
                ToastService.ShowSuccess("Cập nhật phòng/khoa thành công!");
                NavigationManager.NavigateTo("/equipment");
            }
        }
    }
}
