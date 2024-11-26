using EMS.BL.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS.ApiService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MaintenanceHistoryController(IMaintenanceHistoryService maintenanceHistoryService) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<BaseResponseModel>> GetMaintenanceHistories()
		{
			var maintenanceHistories = await maintenanceHistoryService.GetMaintenanceHistories();
			return Ok(new BaseResponseModel { Success = true, Data = maintenanceHistories });
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<BaseResponseModel>> GetMaintenanceHistory(int id)
		{
			var maintenanceHistoryModel = await maintenanceHistoryService.GetMaintenanceHistory(id);

			if (maintenanceHistoryModel == null)
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}
			return Ok(new BaseResponseModel { Success = true, Data = maintenanceHistoryModel });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMaintenanceHistory(int id)
		{
			if (!await maintenanceHistoryService.MaintenanceHistoryModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}
			await maintenanceHistoryService.DeleteMaintenanceHistory(id);
			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateMaintenanceHistory(int id, MaintenanceHistoryModel maintenanceHistoryModel)
		{
			if (id != maintenanceHistoryModel.ID || !await maintenanceHistoryService.MaintenanceHistoryModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Bad request" });
			}

			await maintenanceHistoryService.UpdateMaintenanceHistory(maintenanceHistoryModel);
			return Ok(new BaseResponseModel { Success = true });
		}
	}
}
