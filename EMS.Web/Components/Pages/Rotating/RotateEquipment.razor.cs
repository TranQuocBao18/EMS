using Blazored.Toast.Services;
using EMS.Model.DTOs;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Rotating
{
    public partial class RotateEquipment
    {
        [Parameter]
        public int ID { get; set; }
        public CompletePurchasingRequestDto Model { get; set; } = new();

        [Inject]
        private ApiClient ApiClient { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        private List<DepartmentModel> OldDepartments { get; set; } = new();
        private List<DepartmentModel> NewDepartments { get; set; } = new();
        private List<EquipmentModel> Equipments { get; set; } = new();
        private RotatingRequestModel RotatingRequest { get; set; } = new();


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Model.RequestId = ID;
            
        }


        public async Task Submit()
        {
            var res = await ApiClient.PostAsync<BaseResponseModel, CompletePurchasingRequestDto>($"/api/RotatingRequest/complete", Model);
            if (res != null && res.Success)
            {
                ToastService.ShowSuccess("Luân chuyển thiết bị thành công!");
                NavigationManager.NavigateTo("/rotating/history");
            }
        }
    }
}
