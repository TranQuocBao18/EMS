using System.Text.Json;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;

namespace EMS.Web.Components.Pages
{
	public partial class Home
	{
		private int userCount;
		private int departmentCount;
		private int equipmentCount;
		private int equipmentTypeCount;
		[Inject]
		private ApiClient ApiClient { get; set; }

		protected override async Task OnInitializedAsync()
		{
			base.OnInitializedAsync();
			var userRes = (await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/User/count"));
			if (userRes != null && userRes.Success) 
			{
				userCount = ((JsonElement)userRes.Data).GetInt32();
            }

            var departmentRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/Department/count");
			if (departmentRes != null && departmentRes.Success)
			{
				departmentCount = ((JsonElement)departmentRes.Data).GetInt32();
            }

            var equipmentRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/Equipment/count");
			if (equipmentRes != null && equipmentRes.Success)
			{
				equipmentCount = ((JsonElement)equipmentRes.Data).GetInt32();
            }

            var equipmentTypeRes = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/EquipmentType/count");
			if (equipmentTypeRes != null && equipmentTypeRes.Success)
			{
				equipmentTypeCount = ((JsonElement)equipmentTypeRes.Data).GetInt32();
            }
        }
	}
}
