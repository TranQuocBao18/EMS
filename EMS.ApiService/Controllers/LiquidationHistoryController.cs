using EMS.BL.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS.ApiService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LiquidationHistoryController(ILiquidationHistoryService liquidationHistoryService) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<BaseResponseModel>> GetLiquidationHistories()
		{
			var liquidationHistories = await liquidationHistoryService.GetLiquidationHistories();
			return Ok(new BaseResponseModel { Success = true, Data = liquidationHistories });
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<BaseResponseModel>> GetLiquidationHistory(int id)
		{
			var liquidationHistoryModel = await liquidationHistoryService.GetLiquidationHistory(id);

			if (liquidationHistoryModel == null)
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}
			return Ok(new BaseResponseModel { Success = true, Data = liquidationHistoryModel });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteLiquidationHistory(int id)
		{
			if (!await liquidationHistoryService.LiquidationHistoryModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}
			await liquidationHistoryService.DeleteLiquidationHistory(id);
			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateLiquidationHistory(int id, LiquidationHistoryModel liquidationHistoryModel)
		{
			if (id != liquidationHistoryModel.ID || !await liquidationHistoryService.LiquidationHistoryModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Bad request" });
			}

			await liquidationHistoryService.UpdateLiquidationHistory(liquidationHistoryModel);
			return Ok(new BaseResponseModel { Success = true });
		}
	}
}
