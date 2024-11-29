using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using EMS.Web.Components.BaseComponents;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.EquipmentType
{
    public partial class IndexEquipmentType
    {
		[Inject]
		public ApiClient ApiClient { get; set; }
		public List<EquipmentTypeModel> EquipmentTypeModels { get; set; }
		public AppModal Modal { get; set; }
		public int DeleteID { get; set; }
		[Inject]
		private IToastService ToastService { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			await LoadEquipmentType();
		}

		protected async Task LoadEquipmentType()
		{
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/EquipmentType");
			if (res != null && res.Success)
			{
				EquipmentTypeModels = JsonConvert.DeserializeObject<List<EquipmentTypeModel>>(res.Data.ToString());
			}
		}

		protected async Task HandleDelete()
		{
			var res = await ApiClient.DeleteAsync<BaseResponseModel>($"/api/EquipmentType/{DeleteID}");
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Xoá thiết bị hoàn tất");
				await LoadEquipmentType();
				Modal.Close();
			}
		}
	}
}
