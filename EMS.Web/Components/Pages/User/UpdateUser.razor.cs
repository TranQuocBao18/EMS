using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.User
{
    public partial class UpdateUser : ComponentBase
	{
		[Parameter]
		public int ID { get; set; }
		public UserModel Model { get; set; } = new();
		[Inject]
		private ApiClient ApiClient { get; set; }
		[Inject]
		private IToastService ToastService { get; set; }
		[Inject]
		private NavigationManager NavigationManager { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/User/{ID}");
			if (res != null && res.Success)
			{
				Model = JsonConvert.DeserializeObject<UserModel>(res.Data.ToString());
			}
		}

		public async Task Submit()
		{
			var res = await ApiClient.PutAsync<BaseResponseModel, UserModel>($"/api/User/{ID}", Model);
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Cập nhật người dùng thành công!");
				NavigationManager.NavigateTo("/user");
			}
		}
	}
}
