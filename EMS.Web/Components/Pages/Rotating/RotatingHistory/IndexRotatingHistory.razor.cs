using System.Text;
using BlazorDownloadFile;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Rotating.RotatingHistory
{
    public partial class IndexRotatingHistory
    {
        [Inject]
        public ApiClient ApiClient { get; set; }
		[Inject]
		private IBlazorDownloadFileService DownloadFileService { get; set; }
		public List<RotatingHistoryModel> RotatingHistoryModels { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadHistoryRequest();
        }

        protected async Task LoadHistoryRequest()
        {
            var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/RotatingHistory");
            if (res != null && res.Success)
            {
                RotatingHistoryModels = JsonConvert.DeserializeObject<List<RotatingHistoryModel>>(res.Data.ToString());
            }
        }

		private async Task ExportToCSV()
		{
			var csvData = new StringBuilder();
			csvData.AppendLine("ID,Người yêu cầu,Tên thiết bị,Ngày luân chuyển,Từ Phòng/khoa,Đến Phòng/khoa,Người duyệt cấp 2,Người duyệt cấp 3");

			foreach (var history in RotatingHistoryModels)
			{
				csvData.AppendLine($"{history.ID}," +
								   $"{history.RequestUser.Username}," +
								   $"{history.Equipment.EquipmentName}," +
								   $"{history.RotatingDate:dd/MM/yyyy HH:mm:ss}," +
								   $"{history.ReviewerLv2.Username}," +
								   $"{history.ReviewerLv3.Username}");
			}

			var fileName = $"LiquidationHistory_{DateTime.Now:ddMMyyyy}.csv";
			var bytes = Encoding.UTF8.GetBytes(csvData.ToString());

			// Dùng BlazorDownloadFile để tải file
			await DownloadFileService.DownloadFile(fileName, bytes, "text/csv");
		}
	}
}
