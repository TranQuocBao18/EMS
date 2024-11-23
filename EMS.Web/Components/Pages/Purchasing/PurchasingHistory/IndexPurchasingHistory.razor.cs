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
	}
}
