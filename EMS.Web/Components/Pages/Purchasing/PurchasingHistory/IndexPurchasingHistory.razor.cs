using System.Text;
using BlazorDownloadFile;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Purchasing.PurchasingHistory
{
	public partial class IndexPurchasingHistory
	{
		[Inject]
		public ApiClient ApiClient { get; set; }
		[Inject]
		private IBlazorDownloadFileService DownloadFileService { get; set; }
		public List<PurchasingHistoryModel> PurchasingHistoryModels { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			await LoadHistoryRequest();
		}

		protected async Task LoadHistoryRequest()
		{
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/PurchasingHistory");
			if (res != null && res.Success)
			{
				PurchasingHistoryModels = JsonConvert.DeserializeObject<List<PurchasingHistoryModel>>(res.Data.ToString());
			}
		}

		private async Task ExportToCSV()
		{
			var csvData = new StringBuilder();
			csvData.AppendLine("ID,Ngày mua,Ghi chú,Người duyệt cấp 2,Người duyệt cấp 3,Chi tiết");

			foreach (var history in PurchasingHistoryModels)
			{
				var details = new StringBuilder();
				foreach (var detail in history.PurchasingRequestDetails)
				{
					details.Append($"{detail.EquipmentType.EquipmentTypeName} (Số lượng: {detail.Quantity}, Nhà cung cấp: {detail.Supplier}, Giá: {detail.Price}); ");
				}

				csvData.AppendLine($"{history.ID}," +
								   $"{history.PurchasedDate:dd/MM/yyyy HH:mm:ss}," +
								   $"{history.Notes}," +
								   $"{history.ReviewerLv2?.Username}," +
								   $"{history.ReviewerLv3?.Username}," +
								   $"\"{details.ToString().TrimEnd()}\"");
			}

			var fileName = $"PurchasingHistory_{DateTime.Now:ddMMyyyy}.csv";
			var bytes = Encoding.UTF8.GetBytes(csvData.ToString());

			// Dùng BlazorDownloadFile để tải file
			await DownloadFileService.DownloadFile(fileName, bytes, "text/csv");
		}
	}
}
