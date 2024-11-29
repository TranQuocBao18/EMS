using System.Text;
using BlazorDownloadFile;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;


namespace EMS.Web.Components.Pages.Liquidation.LiquidationHistory
{
	public partial class IndexLiquidationHistory
	{
		[Inject]
		public ApiClient ApiClient { get; set; }
		[Inject]
		private IBlazorDownloadFileService DownloadFileService { get; set; }
		public List<LiquidationHistoryModel> LiquidationHistoryModels { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			await LoadHistory();
		}

		protected async Task LoadHistory()
		{
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/LiquidationHistory");
			if (res != null && res.Success)
			{
				LiquidationHistoryModels = JsonConvert.DeserializeObject<List<LiquidationHistoryModel>>(res.Data.ToString());
			}
		}

		private async Task ExportToCSV()
		{
			var csvData = new StringBuilder();
			csvData.AppendLine("ID,Tên thiết bị,Ngày thanh lý,Người duyệt cấp 2,Người duyệt cấp 3,Ghi chú");

			foreach (var history in LiquidationHistoryModels)
			{
				csvData.AppendLine($"{history.ID}," +
								   $"{history.Equipment.EquipmentName}," +
								   $"{history.LiquidDate:dd/MM/yyyy HH:mm:ss}," +
								   $"{history.ReviewerLv2.Username}," +
								   $"{history.ReviewerLv3.Username}" +
								   $"{history.Notes}");
			}

			var fileName = $"LiquidationHistory_{DateTime.Now:ddMMyyyy}.csv";
			var bytes = Encoding.UTF8.GetBytes(csvData.ToString());

			// Dùng BlazorDownloadFile để tải file
			await DownloadFileService.DownloadFile(fileName, bytes, "text/csv");
		}
	}
}
