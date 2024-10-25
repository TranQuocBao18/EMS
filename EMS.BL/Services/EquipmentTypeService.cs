using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.BL.Repositories;
using EMS.Model.DTOs;
using EMS.Model.Entities;

namespace EMS.BL.Services
{
	public interface IEquipmentTypeService
	{
		Task<List<EquipmentTypeModel>> GetEquipmentTypes();
		Task<EquipmentTypeModel> GetEquipmentType(int id);
		Task UpdateEquipmentType(EquipmentTypeModel equipmentType);
		Task<EquipmentTypeModel> CreateEquipmentType(EquipmentTypeModel equipmentType);
		Task<bool> EquipmentTypeModelExists(int id);
		Task DeleteEquipmentType(int id);
	}
	public class EquipmentTypeService(IEquipmentTypeRepository equipmentTypeRepository) : IEquipmentTypeService
	{
		public Task<EquipmentTypeModel> CreateEquipmentType(EquipmentTypeModel equipmentType)
		{
			return equipmentTypeRepository.CreateEquipmentType(equipmentType);
		}

		public Task DeleteEquipmentType(int id)
		{
			return equipmentTypeRepository.DeleteEquipmentType(id);
		}

		public Task<bool> EquipmentTypeModelExists(int id)
		{
			return equipmentTypeRepository.EquipmentTypeModelExists(id);
		}

		public Task<EquipmentTypeModel> GetEquipmentType(int id)
		{
			return equipmentTypeRepository.GetEquipmentType(id);
		}

		public Task<List<EquipmentTypeModel>> GetEquipmentTypes()
		{
			return equipmentTypeRepository.GetEquipmentTypes();
		}

		public Task UpdateEquipmentType(EquipmentTypeModel equipmentType)
		{
			return equipmentTypeRepository.UpdateEquipmentType(equipmentType);
		}
	}
}
