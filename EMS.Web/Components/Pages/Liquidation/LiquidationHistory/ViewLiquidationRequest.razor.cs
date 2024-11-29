using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Liquidation.LiquidationHistory
{
	public partial class ViewLiquidationRequest
	{
		[Parameter]
		public int ID { get; set; }
		public LiquidationRequestModel Model { get; set; } = new();

		[Inject]
		private ApiClient ApiClient { get; set; }
		[Inject]
		private IToastService ToastService { get; set; }
		[Inject]
		private NavigationManager NavigationManager { get; set; }
		[Inject]
		public AuthenticationStateProvider AuthStateProvider { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/LiquidationRequest/{ID}");
			if (res != null && res.Success)
			{
				Model = JsonConvert.DeserializeObject<LiquidationRequestModel>(res.Data.ToString());
			}
		}
	}
}
