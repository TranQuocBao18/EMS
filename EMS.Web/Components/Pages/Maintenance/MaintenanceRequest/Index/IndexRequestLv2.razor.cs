using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Maintenance.MaintenanceRequest.Index
{
	public partial class IndexRequestLv2
	{
		[Inject]
		public ApiClient ApiClient { get; set; }
		public List<MaintenanceRequestModel> MaintenanceRequestModels { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			await LoadMaintenanceRequest();
		}

		protected async Task LoadMaintenanceRequest()
		{
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/MaintenanceRequest/lv3/pending");
			if (res != null && res.Success)
			{
				MaintenanceRequestModels = JsonConvert.DeserializeObject<List<MaintenanceRequestModel>>(res.Data.ToString());
			}
		}
	}
}
