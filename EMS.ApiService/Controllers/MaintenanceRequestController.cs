using EMS.BL.Services;
using EMS.Model.DTOs;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS.ApiService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MaintenanceRequestController(IMaintenanceRequestService maintenanceRequestService) : ControllerBase
	{
		[HttpPost("create")]
		public async Task<ActionResult<MaintenanceRequestModel>> CreateRequest(MaintenanceRequestModel request)
		{
			await maintenanceRequestService.CreateRequest(request);
			return Ok(new BaseResponseModel { Success = true, ErrorMessage = "Request created successfully" });
		}

		[HttpGet("lv2/pending")]
		public async Task<ActionResult<BaseResponseModel>> GetPendingRequestsLv2()
		{
			var requests = await maintenanceRequestService.GetPendingRequestsLv2();

			return Ok(new BaseResponseModel { Success = true, Data = requests });
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<MaintenanceRequestModel>> GetRequest(int id)
		{
			var requests = await maintenanceRequestService.GetRequest(id);
			if (requests == null)
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}

			return Ok(new BaseResponseModel { Success = true, Data = requests });
		}

		[HttpPost("lv2/approve")]
		public async Task<ActionResult<BaseResponseModel>> ApproveRequestLv2(ApproveRequestDto dto)
		{
			var request = await maintenanceRequestService.ApproveRequestLv2(dto);

			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpGet("lv3/pending")]
		public async Task<ActionResult<BaseResponseModel>> GetPendingRequestsLv3()
		{
			var requests = await maintenanceRequestService.GetPendingRequestsLv3();

			return Ok(new BaseResponseModel { Success = true, Data = requests });
		}

		[HttpPost("lv3/approve")]
		public async Task<ActionResult<BaseResponseModel>> ApproveRequestLv3(ApproveRequestDto dto)
		{
			var request = await maintenanceRequestService.ApproveRequestLv3(dto);

			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpGet("approved")]
		public async Task<ActionResult<BaseResponseModel>> GetApprovedRequest()
		{
			var requests = await maintenanceRequestService.GetApprovedRequest();

			return Ok(new BaseResponseModel { Success = true, Data = requests });
		}

		[HttpPost("{id}/complete-approval")]
		public async Task<ActionResult<BaseResponseModel>> CompleteApproval(int id)
		{
			var request = await maintenanceRequestService.CompleteApproval(id);

			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpPost("complete-maintenance")]
		public async Task<ActionResult<BaseResponseModel>> CompleteMaintenance(CompletePurchasingRequestDto dto)
		{
			var request = await maintenanceRequestService.CompleteMaintenance(dto);

			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpGet("own/{id}")]
		public async Task<ActionResult<MaintenanceRequestModel>> GetRequestsByUserId(int id)
		{
			var requests = await maintenanceRequestService.GetRequestsByUserId(id);
			if (requests == null)
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}

			return Ok(new BaseResponseModel { Success = true, Data = requests });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRequest(int id)
		{
			if (!await maintenanceRequestService.MaintenanceRequestModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}
			await maintenanceRequestService.DeleteRequest(id);
			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateRequest(int id, MaintenanceRequestModel maintenanceRequest)
		{
			if (id != maintenanceRequest.ID || !await maintenanceRequestService.MaintenanceRequestModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Bad request" });
			}

			await maintenanceRequestService.UpdateRequest(maintenanceRequest);
			return Ok(new BaseResponseModel { Success = true });
		}
	}
}
