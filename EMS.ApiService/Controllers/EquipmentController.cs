using EMS.BL.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS.ApiService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EquipmentController(IEquipmentService equipmentService) : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<BaseResponseModel>> GetEquipments()
		{
			var equipments = await equipmentService.GetEquipments();
			return Ok(new BaseResponseModel { Success = true, Data = equipments });
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<BaseResponseModel>> GetEquipment(int id)
		{
			var equipmentModel = await equipmentService.GetEquipment(id);

			if (equipmentModel == null)
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}
			return Ok(new BaseResponseModel { Success = true, Data = equipmentModel });
		}

		[HttpPost]
		public async Task<ActionResult<EquipmentModel>> CreateEquipment(EquipmentModel equipment)
		{
			await equipmentService.CreateEquipment(equipment);
			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateEquipment(int id, EquipmentModel equipment)
		{
			if (id != equipment.ID || !await equipmentService.EquipmentModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Bad request" });
			}

			await equipmentService.UpdateEquipment(equipment);
			return Ok(new BaseResponseModel { Success = true });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteEquipment(int id)
		{
			if (!await equipmentService.EquipmentModelExists(id))
			{
				return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
			}
			await equipmentService.DeleteEquipment(id);
			return Ok(new BaseResponseModel { Success = true });
		}
	}
}
