using EMS.BL.Services;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RotatingHistoryController(IRotatingHistoryService rotatingHistoryService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<BaseResponseModel>> GetRotatingHistories()
        {
            var rotatingHistories = await rotatingHistoryService.GetRotatingHistories();
            return Ok(new BaseResponseModel { Success = true, Data = rotatingHistories });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponseModel>> GetDepartment(int id)
        {
            var rotatingHistoryModel = await rotatingHistoryService.GetRotatingHistory(id);

            if (rotatingHistoryModel == null)
            {
                return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
            }
            return Ok(new BaseResponseModel { Success = true, Data = rotatingHistoryModel });
        }
    }
}
