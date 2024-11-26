using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Maintenance.MaintenanceRequest.Index
{
	public partial class IndexRequestApproved
	{
		[Inject]
		public ApiClient ApiClient { get; set; }
		[Inject]
		private IToastService ToastService { get; set; }
		public List<MaintenanceRequestModel> MaintenanceRequestModels { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			await LoadMaintenanceRequest();
		}

		protected async Task LoadMaintenanceRequest()
		{
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/MaintenanceRequest/approved");
			if (res != null && res.Success)
			{
				MaintenanceRequestModels = JsonConvert.DeserializeObject<List<MaintenanceRequestModel>>(res.Data.ToString());
			}
		}

		protected async Task StartMaintenance(int requestId)
		{
			var res = await ApiClient.PostAsync<BaseResponseModel, int>($"/api/MaintenanceRequest/{requestId}/complete-approval", requestId);
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Đã bắt đầu quá trình bảo trì thiết bị!");
				await LoadMaintenanceRequest();
			}
		}
	}
}
