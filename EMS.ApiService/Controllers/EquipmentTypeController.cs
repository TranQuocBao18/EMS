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
	public class EquipmentTypeController(IEquipmentTypeService equipmentTypeService) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<BaseResponseModel>> GetEquipmentTypes()
		{
			var equipmentTypes = await equipmentTypeService.GetEquipmentTypes();
			return Ok(new BaseResponseModel { Success = true, Data = equipmentTypes });
		}

		[HttpGet("count")]
		public async Task<ActionResult<BaseResponseModel>> GetEquipmentTypesAmount()
		{
			var users = await equipmentTypeService.GetEquipmentTypesAmount();
			return Ok(new BaseResponseModel { Success = true, Data = users });
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<BaseResponseModel>> GetEquipmentType(int id)
		{
			var equipmentTypeModel = await equipmentTypeService.GetEquipmentType(id);

			if (equipmentTypeModel == null)
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}
			return Ok(new BaseResponseModel { Success = true, Data = equipmentTypeModel });
		}

		[HttpPost]
		public async Task<ActionResult<EquipmentTypeModel>> CreateEquipmentType(EquipmentTypeModel equipmentType)
		{
			await equipmentTypeService.CreateEquipmentType(equipmentType);
			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateEquipmentType(int id, EquipmentTypeModel equipmentType)
		{
			if (id != equipmentType.ID || !await equipmentTypeService.EquipmentTypeModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Bad request" });
			}

			await equipmentTypeService.UpdateEquipmentType(equipmentType);
			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteEquipmentType(int id)
		{
			if (!await equipmentTypeService.EquipmentTypeModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}
			await equipmentTypeService.DeleteEquipmentType(id);
			return Ok(new BaseResponseModel { Success = true });
		}
	}
}
