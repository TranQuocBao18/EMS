using Blazored.Toast.Services;
using EMS.Model.DTOs;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;

namespace EMS.Web.Components.Pages.Department
{
    public partial class CreateDepartment
    {
		public DepartmentDto Model { get; set; } = new();

		[Inject]
		private ApiClient ApiClient { get; set; }
		[Inject]
		private IToastService ToastService { get; set; }
		[Inject]
		private NavigationManager NavigationManager { get; set; }

		public async Task Submit()
		{
			var res = await ApiClient.PostAsync<BaseResponseModel, DepartmentDto>("/api/Department", Model);
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Tạo phòng/khoa thành công!");
				NavigationManager.NavigateTo("/department");
			}
		}
	}
}
