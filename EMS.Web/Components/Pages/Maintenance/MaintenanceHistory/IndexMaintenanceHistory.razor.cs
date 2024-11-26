using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Maintenance.MaintenanceHistory
{
	public partial class IndexMaintenanceHistory
	{
		[Inject]
		public ApiClient ApiClient { get; set; }
		public List<MaintenanceHistoryModel> MaintenanceHistoryModels { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			await LoadHistoryRequest();
		}

		protected async Task LoadHistoryRequest()
		{
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/MaintenanceHistory");
			if (res != null && res.Success)
			{
				MaintenanceHistoryModels = JsonConvert.DeserializeObject<List<MaintenanceHistoryModel>>(res.Data.ToString());
			}
		}
	}
}
