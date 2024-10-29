using Blazored.Toast.Services;
using EMS.Model.DTOs;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Department
{
	public partial class UpdateDepartment: ComponentBase
	{
		[Parameter]
		public int ID { get; set; }
		public DepartmentModel Model { get; set; } = new();
		public DepartmentDto ModelDTO { get; set; } = new();

		[Inject]
		private ApiClient ApiClient { get; set; }
		[Inject]
		private IToastService ToastService { get; set; }
		[Inject]
		private NavigationManager NavigationManager { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>($"/api/Department/{ID}");
			if (res != null && res.Success)
			{
				Model = JsonConvert.DeserializeObject<DepartmentModel>(res.Data.ToString());
			}
		}

		public async Task Submit()
		{
			ModelDTO.ID = Model.ID;
			ModelDTO.DepartmentName = Model.DepartmentName;
			var res = await ApiClient.PutAsync<BaseResponseModel, DepartmentDto>($"/api/Department/{ID}", ModelDTO);
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Cập nhật phòng/khoa thành công!");
				NavigationManager.NavigateTo("/department");
			}
		}
	}
}
