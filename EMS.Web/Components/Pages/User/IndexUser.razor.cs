using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using EMS.Web.Components.BaseComponents;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.User
{
    public partial class IndexUser
	{
        [Inject]
        public ApiClient ApiClient { get; set; }
        public List<UserModel> UserModels { get; set; }
        public AppModal Modal { get; set; }
        public AppModal LockModal { get; set; }
        public int DeleteID { get; set; }
        public string UserName { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadUser();
        }

        protected async Task LoadUser()
        {
            var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/User");
            if (res != null && res.Success)
            {
                UserModels = JsonConvert.DeserializeObject<List<UserModel>>(res.Data.ToString());
            }
        }

        protected async Task HandleDelete()
        {
            var res = await ApiClient.DeleteAsync<BaseResponseModel>($"/api/User/{DeleteID}");
            if (res != null && res.Success)
            {
                ToastService.ShowSuccess("Xoá người dùng hoàn tất");
                await LoadUser();
                Modal.Close();
            }
        }

        protected async Task HandleLock()
        {
            var res = await ApiClient.PatchAsync<BaseResponseModel, object>($"/api/User/ToggleUserLockStatus?username={UserName}", null);
            if (res != null && res.Success)
            {
                ToastService.ShowSuccess("Cập nhật trạng thái khoá của người dùng thành công");
                await LoadUser();
                LockModal.Close();
            }
        }
    }
}
