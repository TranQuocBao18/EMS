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
	public class LiquidationRequestController(ILiquidationRequestService liquidationRequestService) : ControllerBase
	{
		[HttpPost("create")]
		public async Task<ActionResult<LiquidationRequestModel>> CreateRequest(LiquidationRequestModel request)
		{
			await liquidationRequestService.CreateRequest(request);
			return Ok(new BaseResponseModel { Success = true, ErrorMessage = "Request created successfully" });
		}

		[HttpGet("lv2/pending")]
		public async Task<ActionResult<BaseResponseModel>> GetPendingRequestsLv2()
		{
			var requests = await liquidationRequestService.GetPendingRequestsLv2();

			return Ok(new BaseResponseModel { Success = true, Data = requests });
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<LiquidationRequestModel>> GetRequest(int id)
		{
			var requests = await liquidationRequestService.GetRequest(id);
			if (requests == null)
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}

			return Ok(new BaseResponseModel { Success = true, Data = requests });
		}

		[HttpPost("lv2/approve")]
		public async Task<ActionResult<BaseResponseModel>> ApproveRequestLv2(ApproveRequestDto dto)
		{
			var request = await liquidationRequestService.ApproveRequestLv2(dto);

			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpGet("lv3/pending")]
		public async Task<ActionResult<BaseResponseModel>> GetPendingRequestsLv3()
		{
			var requests = await liquidationRequestService.GetPendingRequestsLv3();

			return Ok(new BaseResponseModel { Success = true, Data = requests });
		}

		[HttpPost("lv3/approve")]
		public async Task<ActionResult<BaseResponseModel>> ApproveRequestLv3(ApproveRequestDto dto)
		{
			var request = await liquidationRequestService.ApproveRequestLv3(dto);

			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpGet("approved")]
		public async Task<ActionResult<BaseResponseModel>> GetApprovedRequest()
		{
			var requests = await liquidationRequestService.GetApprovedRequest();

			return Ok(new BaseResponseModel { Success = true, Data = requests });
		}

		[HttpPost("complete")]
		public async Task<ActionResult<BaseResponseModel>> CompleteRequest(CompletePurchasingRequestDto dto)
		{
			var request = await liquidationRequestService.CompleteRequest(dto);

			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpGet("own/{id}")]
		public async Task<ActionResult<LiquidationRequestModel>> GetRequestsByUserId(int id)
		{
			var requests = await liquidationRequestService.GetRequestsByUserId(id);
			if (requests == null)
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}

			return Ok(new BaseResponseModel { Success = true, Data = requests });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRequest(int id)
		{
			if (!await liquidationRequestService.LiquidationRequestModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}
			await liquidationRequestService.DeleteRequest(id);
			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateRequest(int id, LiquidationRequestModel liquidationRequestModel)
		{
			if (id != liquidationRequestModel.ID || !await liquidationRequestService.LiquidationRequestModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Bad request" });
			}

			await liquidationRequestService.UpdateRequest(liquidationRequestModel);
			return Ok(new BaseResponseModel { Success = true });
		}
	}
}
