using EMS.BL.Services;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS.ApiService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController(IRoleService roleService) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<BaseResponseModel>> GetRoles()
		{
			var roles = await roleService.GetRoles();
			return Ok(new BaseResponseModel { Success = true, Data = roles });
		}
	}
}
