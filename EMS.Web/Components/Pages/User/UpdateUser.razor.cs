using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using EMS.Model.Models.User;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.User
{
    public partial class UpdateUser : ComponentBase
	{
		[Parameter]
		public int ID { get; set; }
		public UserModel Model { get; set; } = new();
        public List<RoleModelBlazor> AllRoles { get; set; } = new();
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

            var rolesRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/Role");
            if (rolesRes != null && rolesRes.Success)
            {
                // Chuyển đổi sang RoleModelBlazor
                var roles = JsonConvert.DeserializeObject<List<RoleModel>>(rolesRes.Data.ToString());
                AllRoles = roles.Select(role => new RoleModelBlazor { ID = role.ID, RoleName = role.RoleName }).ToList();

                // Đánh dấu vai trò mà người dùng hiện tại đã có
                foreach (var role in Model.UserRoles)
                {
                    var foundRole = AllRoles.FirstOrDefault(r => r.ID == role.RoleID);
                    if (foundRole != null)
                    {
                        foundRole.IsSelected = true;
                    }
                }
            }
        }

		public async Task Submit()
		{
            Model.UserRoles = AllRoles
            .Where(r => r.IsSelected)
            .Select(r => new UserRoleModel { RoleID = r.ID })
            .ToList();

            var res = await ApiClient.PutAsync<BaseResponseModel, UserModel>($"/api/User/{ID}", Model);
			if (res != null && res.Success)
			{
				ToastService.ShowSuccess("Cập nhật người dùng thành công!");
				NavigationManager.NavigateTo("/user");
			}
		}
	}
}
