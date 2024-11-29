using System.Text;
using BlazorDownloadFile;
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
		[Inject]
		private IBlazorDownloadFileService DownloadFileService { get; set; }
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
		private async Task ExportToCSV()
		{
			var csvData = new StringBuilder();
			csvData.AppendLine("ID,Tên thiết bị,Ngày bắt đầu bảo trì,Ngày kết thúc bảo trì,Người duyệt cấp 2,Người duyệt cấp 3,Ghi chú");

			foreach (var history in MaintenanceHistoryModels)
			{
				csvData.AppendLine($"{history.ID}," +
								   $"{history.Equipment.EquipmentName}," +
								   $"{history.MaintenanceStartDate:dd/MM/yyyy HH:mm:ss}," +
								   $"{history.MaintenanceEndDate:dd/MM/yyyy HH:mm:ss}," +
								   $"{history.ReviewerLv2.Username}," +
								   $"{history.ReviewerLv3.Username}," +
								   $"{history.Notes}");
			}

			var fileName = $"MaintenanceHistory_{DateTime.Now:ddMMyyyy}.csv";
			var bytes = Encoding.UTF8.GetBytes(csvData.ToString());

			// Dùng BlazorDownloadFile để tải file
			await DownloadFileService.DownloadFile(fileName, bytes, "text/csv");
		}
	}
}
