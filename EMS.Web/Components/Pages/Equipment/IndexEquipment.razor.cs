using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using EMS.Web.Components.BaseComponents;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Equipment
{
	public partial class IndexEquipment
	{
		[Inject]
		public ApiClient ApiClient { get; set; }
		public List<EquipmentModel> EquipmentModels { get; set; }
		public AppModal Modal { get; set; }
		public int DeleteID { get; set; }
		[Inject]
		private IToastService ToastService { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			await LoadEquipment();
		}

		protected async Task LoadEquipment()
		{
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/Equipment");
			if (res != null && res.Success)
			{
				EquipmentModels = JsonConvert.DeserializeObject<List<EquipmentModel>>(res.Data.ToString());
			}
		}

		protected async Task HandleDelete()
		{
			var res = await ApiClient.DeleteAsync<BaseResponseModel>($"/api/Equipment/{DeleteID}");
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Xoá thiết bị hoàn tất");
				await LoadEquipment();
				Modal.Close();
			}
		}
	}
}
