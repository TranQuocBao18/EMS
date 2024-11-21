﻿using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace EMS.Web.Components.Pages.Purchasing.PurchasingRequest.Index
{
	public partial class IndexRequestLv1
	{
		[Inject]
		public ApiClient ApiClient { get; set; }
		public List<PurchasingRequestModel> PurchasingRequestModels { get; set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();
			await LoadPurchasingRequest();
		}

		protected async Task LoadPurchasingRequest()
		{
			var res = await ApiClient.GetFromJsonAsync<BaseResponseModel>("/api/RotatingRequest/lv2/pending");
			if (res != null && res.Success)
			{
				PurchasingRequestModels = JsonConvert.DeserializeObject<List<PurchasingRequestModel>>(res.Data.ToString());
			}
		}
	}
}
