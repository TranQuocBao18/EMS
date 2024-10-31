using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.RotatingRequest.Index
{
    public partial class IndexRequestApproved
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
            var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/RotatingRequest/approved");
            if (res != null && res.Success)
            {
                RotatingRequestModels = JsonConvert.DeserializeObject<List<RotatingRequestModel>>(res.Data.ToString());
            }
        }
    }
}
