using Blazored.Toast.Services;
using EMS.Model.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using EMS.Model.Models.Others;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Rotating.RotatingHistory
{
	public partial class ViewRotatingRequest
	{
		[Parameter]
		public int ID { get; set; }
		public RotatingRequestModel Model { get; set; } = new();

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
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/RotatingRequest/{ID}");
			if (res != null && res.Success)
			{
				Model = JsonConvert.DeserializeObject<RotatingRequestModel>(res.Data.ToString());
			}
		}
	}
}
