using EMS.BL.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS.ApiService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PurchasingHistoryController(IPurchasingHistoryService purchasingHistoryService) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<BaseResponseModel>> GetPurchasingHistories()
		{
			var purchasingHistories = await purchasingHistoryService.GetPurchasingHistories();
			return Ok(new BaseResponseModel { Success = true, Data = purchasingHistories });
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<BaseResponseModel>> GetPurchasingHistory(int id)
		{
			var purchasingHistoryModel = await purchasingHistoryService.GetPurchasingHistory(id);

			if (purchasingHistoryModel == null)
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}
			return Ok(new BaseResponseModel { Success = true, Data = purchasingHistoryModel });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePurchasingHistory(int id)
		{
			if (!await purchasingHistoryService.PurchasingHistoryModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}
			await purchasingHistoryService.DeletePurchasingHistory(id);
			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdatePurchasingHistory(int id, PurchasingHistoryModel purchasingHistoryModel)
		{
			if (id != purchasingHistoryModel.ID || !await purchasingHistoryService.PurchasingHistoryModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Bad request" });
			}

			await purchasingHistoryService.UpdatePurchasingHistory(purchasingHistoryModel);
			return Ok(new BaseResponseModel { Success = true });
		}
	}
}
