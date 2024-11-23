using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Purchasing.PurchasingHistory
{
	public partial class ViewPurchasingRequest
	{
		[Parameter]
		public int ID { get; set; }
		public PurchasingRequestModel Model { get; set; } = new PurchasingRequestModel
		{
			PurchasingRequestDetails = new List<PurchasingRequestDetailModel>()
		};

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
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/PurchasingRequest/{ID}");
			if (res != null && res.Success)
			{
				Model = JsonConvert.DeserializeObject<PurchasingRequestModel>(res.Data.ToString());
			}
		}
	}
}
