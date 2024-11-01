using Blazored.Toast.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using EMS.Web.Components.BaseComponents;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Rotating.RotatingRequest.Index
{
    public partial class IndexRequestLv1
    {
        [Inject]
        public ApiClient ApiClient { get; set; }
        public List<RotatingRequestModel> RotatingRequestModels { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadRotatingRequest();
        }

        protected async Task LoadRotatingRequest()
        {
            var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/RotatingRequest/lv2/pending");
            if (res != null && res.Success)
            {
                RotatingRequestModels = JsonConvert.DeserializeObject<List<RotatingRequestModel>>(res.Data.ToString());
            }
        }
    }
}
