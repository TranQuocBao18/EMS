using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.EquipmentType
{
	public partial class CreateEquipmentType
	{
		public EquipmentTypeModel Model { get; set; } = new();

		[Inject]
		private ApiClient ApiClient { get; set; }
		[Inject]
		private IToastService ToastService { get; set; }
		[Inject]
		private NavigationManager NavigationManager { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
		}

		public async Task Submit()
		{
			var res = await ApiClient.PostAsync<BaseResponseModel, EquipmentTypeModel>("/api/EquipmentType", Model);
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Tạo loại thiết bị thành công!");
				NavigationManager.NavigateTo("/equipmenttype");
			}
		}
	}
}
