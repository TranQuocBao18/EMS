using Blazored.Toast.Services;
using EMS.Model.DTOs;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Equipment
{
	public partial class CreateEquipment
	{
		public EquipmentModel Model { get; set; } = new();

		[Inject]
		private ApiClient ApiClient { get; set; }
		[Inject]
		private IToastService ToastService { get; set; }
		[Inject]
		private NavigationManager NavigationManager { get; set; }
        private List<DepartmentModel> Departments { get; set; } = new();
        private List<EquipmentTypeModel> EquipmentTypes { get; set; } = new();
        private List<UserModel> Users { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

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

            var UsersRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/User");
            if (UsersRes != null && UsersRes.Success)
            {
                Users = JsonConvert.DeserializeObject<List<UserModel>>(UsersRes.Data.ToString());
            }

        }

        public async Task Submit()
		{
			var res = await ApiClient.PostAsync<BaseResponseModel, EquipmentModel>("/api/Equipment", Model);
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Tạo thiết bị thành công!");
				NavigationManager.NavigateTo("/equipment");
			}
		}
	}
}
