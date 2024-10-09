using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models;
using Microsoft.AspNetCore.Components;

namespace EMS.Web.Components.Pages.Product
{
    public partial class CreateProduct
    {
        public ProductModel Model { get; set; } = new();

        [Inject]
        private ApiClient ApiClient { get; set; }
        [Inject]
        private IToastService ToastService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        public async Task Submit()
        {
            var res = await ApiClient.PostAsync<BaseResponseModel, ProductModel>("/api/Product", Model);
            if (res != null && res.Success)
            {
                ToastService.ShowSuccess("Create product successfully");
                NavigationManager.NavigateTo("/product");
            }
        }
    }
}
