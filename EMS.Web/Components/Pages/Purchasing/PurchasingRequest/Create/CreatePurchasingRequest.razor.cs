using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Security.Claims;

namespace EMS.Web.Components.Pages.Purchasing.PurchasingRequest.Create
{
	public partial class CreatePurchasingRequest
	{
        private PurchasingRequestModel Model = new PurchasingRequestModel
        {
            PurchasingRequestDetails = new List<PurchasingRequestDetailModel>()
        };
        private List<EquipmentTypeModel> EquipmentTypes = new();

        [Inject]
        private ApiClient ApiClient { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        public UserModel User { get; set; } = new UserModel();
        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadUserFromToken();

            var EquipmentTypeRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/EquipmentType");
            if (EquipmentTypeRes != null && EquipmentTypeRes.Success)
            {
                EquipmentTypes = JsonConvert.DeserializeObject<List<EquipmentTypeModel>>(EquipmentTypeRes.Data.ToString());

            }
        }

        protected async Task LoadUserFromToken()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var userClaims = authState.User;

            if (userClaims.Identity.IsAuthenticated)
            {
                User.Username = userClaims.FindFirst(ClaimTypes.Name)?.Value;
                // Nếu bạn có thêm các thông tin khác như ID, email, role, thì trích xuất từ claims:
                User.ID = int.Parse(userClaims.FindFirst("UserId")?.Value); // Giả sử bạn lưu UserId trong claim
            }
            else
            {
                ToastService.ShowError("Không thể lấy thông tin người dùng.");
            }
        }

        private void AddDetail()
        {
            Model.PurchasingRequestDetails.Add(new PurchasingRequestDetailModel());
        }

        private void RemoveDetail(PurchasingRequestDetailModel detail)
        {
            Model.PurchasingRequestDetails.Remove(detail);
        }

        public async Task Submit()
        {
            Model.UserId = User.ID;
            if (!Model.PurchasingRequestDetails.Any())
            {
                // Nếu không có thiết bị nào được chọn
                ToastService.ShowError("Vui lòng thêm ít nhất một loại thiết bị.");
                return;
            }
            var res = await ApiClient.PostAsync<BaseResponseModel, PurchasingRequestModel>("/api/PurchasingRequest/create", Model);
            if (res != null && res.Success)
            {
                ToastService.ShowSuccess("Tạo yêu cầu thành công!");
                NavigationManager.NavigateTo("/purchasing/ownrequest");
            }
        }
    }
}

