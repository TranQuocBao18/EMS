using Blazored.Toast.Services;
using EMS.Model.DTOs;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;

namespace EMS.Web.Components.Pages.Liquidation
{
	public partial class LiquidateEquipment
	{
		[Parameter]
		public int ID { get; set; }
		public CompletePurchasingRequestDto Model { get; set; } = new();

		[Inject]
		private ApiClient ApiClient { get; set; }
		[Inject]
		private IToastService ToastService { get; set; }
		[Inject]
		private NavigationManager NavigationManager { get; set; }


		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			Model.RequestId = ID;
		}


		public async Task Submit()
		{
			var res = await ApiClient.PostAsync<BaseResponseModel, CompletePurchasingRequestDto>($"/api/LiquidationRequest/complete", Model);
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Hoàn tất quá trình thanh lý thiết bị!");
				NavigationManager.NavigateTo("/liquidation/history");
			}
		}
	}
}
