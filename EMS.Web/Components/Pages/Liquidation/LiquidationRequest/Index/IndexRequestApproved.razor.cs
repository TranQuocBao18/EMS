using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Liquidation.LiquidationRequest.Index
{
	public partial class IndexRequestApproved
	{
		[Inject]
		public ApiClient ApiClient { get; set; }
		public List<LiquidationRequestModel> LiquidationRequestModels { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			await LoadLiquidationRequest();
		}

		protected async Task LoadLiquidationRequest()
		{
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/LiquidationRequest/approved");
			if (res != null && res.Success)
			{
				LiquidationRequestModels = JsonConvert.DeserializeObject<List<LiquidationRequestModel>>(res.Data.ToString());
			}
		}
	}
}
