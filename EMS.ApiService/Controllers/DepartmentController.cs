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
	public class DepartmentController(IDepartmentService departmentService) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<BaseResponseModel>> GetDepartments()
		{
			var departments = await departmentService.GetDepartments();
			return Ok(new BaseResponseModel { Success = true, Data = departments });
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<BaseResponseModel>> GetDepartment(int id)
		{
			var departmentModel = await departmentService.GetDepartment(id);

			if (departmentModel == null)
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}
			return Ok(new BaseResponseModel { Success = true, Data = departmentModel });
		}

		[HttpPost]
		public async Task<ActionResult<DepartmentModel>> CreateDepartment(DepartmentDTO departmentDTO)
		{
			await departmentService.CreateDepartment(departmentDTO);
			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateDepartment(int id, DepartmentDTO departmentDTO)
		{
			if (id != departmentDTO.ID || !await departmentService.DepartmentModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Bad request" });
			}

			await departmentService.UpdateDepartment(departmentDTO);
			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteDepartment(int id)
		{
			if (!await departmentService.DepartmentModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}
			await departmentService.DeleteDepartment(id);
			return Ok(new BaseResponseModel { Success = true });
		}
	}
}
