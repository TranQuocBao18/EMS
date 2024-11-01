using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Rotating.RotatingHistory
{
    public partial class IndexRotatingHistory
    {
        [Inject]
        public ApiClient ApiClient { get; set; }
        public List<RotatingHistoryModel> RotatingHistoryModels { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadHistoryRequest();
        }

        protected async Task LoadHistoryRequest()
        {
            var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/RotatingHistory");
            if (res != null && res.Success)
            {
                RotatingHistoryModels = JsonConvert.DeserializeObject<List<RotatingHistoryModel>>(res.Data.ToString());
            }
        }
    }
}
