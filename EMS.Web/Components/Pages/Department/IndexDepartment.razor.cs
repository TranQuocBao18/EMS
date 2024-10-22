using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using EMS.Web.Components.BaseComponents;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Department
{
	public partial class IndexDepartment
	{
        [Inject]
        public ApiClient ApiClient { get; set; }
        public List<DepartmentModel> DepartmentModels { get; set; }
        public AppModal Modal { get; set; }
        public int DeleteID { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadProduct();
        }

        protected async Task LoadProduct()
        {
            var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/Department");
            if (res != null && res.Success)
            {
                DepartmentModels = JsonConvert.DeserializeObject<List<DepartmentModel>>(res.Data.ToString());
            }
        }

        protected async Task HandleDelete()
        {
            var res = await ApiClient.DeleteAsync<BaseResponseModel>($"/api/Department/{DeleteID}");
            if (res != null && res.Success)
            {
                ToastService.ShowSuccess("Xoá phòng/khoa thành công!");
                await LoadProduct();
                Modal.Close();
            }
        }
    }
}
