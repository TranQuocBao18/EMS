using Azure.Core;
using EMS.BL.Services;
using EMS.Model.DTOs;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RotatingRequestController(IRotatingRequestService rotatingRequestService) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<ActionResult<RotatingRequestModel>> CreateRequest(RotatingRequestModel request)
        {
            await rotatingRequestService.CreateRequest(request);
            return Ok(new { message = "Request created successfully" });
        }

        [HttpGet("lv2/pending")]
        public async Task<ActionResult<BaseResponseModel>> GetPendingRequestsLv2()
        {
            var requests = await rotatingRequestService.GetPendingRequestsLv2();

            return Ok(new BaseResponseModel { Success = true, Data = requests });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RotatingRequestModel>> GetRequest(int id)
        {
            var requests = await rotatingRequestService.GetRequest(id);
            if (requests == null)
            {
                return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
            }

            return Ok(new BaseResponseModel { Success = true, Data = requests });
        }

        [HttpPost("lv2/approve")]
        public async Task<ActionResult<BaseResponseModel>> ApproveRequestLv2(ApproveRequestDto dto)
        {
            var request = await rotatingRequestService.ApproveRequestLv2(dto);
            
            return Ok(new BaseResponseModel { Success = true });
        }

        [HttpGet("lv3/pending")]
        public async Task<ActionResult<BaseResponseModel>> GetPendingRequestsLv3()
        {
            var requests = await rotatingRequestService.GetPendingRequestsLv3();

            return Ok(new BaseResponseModel { Success = true, Data = requests });
        }

        [HttpPost("lv3/approve")]
        public async Task<ActionResult<BaseResponseModel>> ApproveRequestLv3(ApproveRequestDto dto)
        {
            var request = await rotatingRequestService.ApproveRequestLv3(dto);
            
            return Ok(new BaseResponseModel { Success = true });
        }

        [HttpGet("approved")]
        public async Task<ActionResult<BaseResponseModel>> GetApprovedRequest()
        {
            var requests = await rotatingRequestService.GetApprovedRequest();

            return Ok(new BaseResponseModel { Success = true, Data = requests });
        }

        [HttpPost("complete")]
        public async Task<ActionResult<BaseResponseModel>> CompleteRequest(CompleteRequestDto dto)
        {
            var request = await rotatingRequestService.CompleteRequest(dto);

            return Ok(new BaseResponseModel { Success = true });
        }
    }
}
