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
	public class PurchasingRequestController(IPurchasingRequestService purchasingRequestService) : ControllerBase
	{
		[HttpPost("create")]
		public async Task<ActionResult<PurchasingRequestModel>> CreateRequest(PurchasingRequestModel request)
		{
			await purchasingRequestService.CreateRequest(request);
			return Ok(new BaseResponseModel { Success = true, ErrorMessage = "Request created successfully" });
		}

		[HttpGet("lv2/pending")]
		public async Task<ActionResult<BaseResponseModel>> GetPendingRequestsLv2()
		{
			var requests = await purchasingRequestService.GetPendingRequestsLv2();

			return Ok(new BaseResponseModel { Success = true, Data = requests });
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<PurchasingRequestModel>> GetRequest(int id)
		{
			var requests = await purchasingRequestService.GetRequest(id);
			if (requests == null)
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}

			return Ok(new BaseResponseModel { Success = true, Data = requests });
		}

		[HttpPost("lv2/approve")]
		public async Task<ActionResult<BaseResponseModel>> ApproveRequestLv2(ApproveRequestDto dto)
		{
			var request = await purchasingRequestService.ApproveRequestLv2(dto);

			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpGet("lv3/pending")]
		public async Task<ActionResult<BaseResponseModel>> GetPendingRequestsLv3()
		{
			var requests = await purchasingRequestService.GetPendingRequestsLv3();

			return Ok(new BaseResponseModel { Success = true, Data = requests });
		}

		[HttpPost("lv3/approve")]
		public async Task<ActionResult<BaseResponseModel>> ApproveRequestLv3(ApproveRequestDto dto)
		{
			var request = await purchasingRequestService.ApproveRequestLv3(dto);

			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpGet("approved")]
		public async Task<ActionResult<BaseResponseModel>> GetApprovedRequest()
		{
			var requests = await purchasingRequestService.GetApprovedRequest();

			return Ok(new BaseResponseModel { Success = true, Data = requests });
		}

		[HttpPost("complete")]
		public async Task<ActionResult<BaseResponseModel>> CompleteRequest(CompleteRequestDto dto)
		{
			var request = await purchasingRequestService.CompleteRequest(dto);

			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpGet("own/{id}")]
		public async Task<ActionResult<PurchasingRequestModel>> GetRequestsByUserId(int id)
		{
			var requests = await purchasingRequestService.GetRequestsByUserId(id);
			if (requests == null)
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}

			return Ok(new BaseResponseModel { Success = true, Data = requests });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteRequest(int id)
		{
			if (!await purchasingRequestService.PurchasingRequestModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}
			await purchasingRequestService.DeleteRequest(id);
			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateRequest(int id, PurchasingRequestModel PurchasingRequestModel)
		{
			if (id != PurchasingRequestModel.ID || !await purchasingRequestService.PurchasingRequestModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Bad request" });
			}

			await purchasingRequestService.UpdateRequest(PurchasingRequestModel);
			return Ok(new BaseResponseModel { Success = true });
		}
	}
}
