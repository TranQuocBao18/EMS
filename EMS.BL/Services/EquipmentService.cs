using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.BL.Repositories;
using EMS.Model.Entities;

namespace EMS.BL.Services
{
	public interface IEquipmentService
	{
		Task<List<EquipmentModel>> GetEquipments();
		Task<EquipmentModel> GetEquipment(int id);
		Task UpdateEquipment(EquipmentModel equipment);
		Task<EquipmentModel> CreateEquipment(EquipmentModel equipment);
		Task<bool> EquipmentModelExists(int id);
		Task DeleteEquipment(int id);
	}

	public class EquipmentService(IEquipmentRepository equipmentRepository) : IEquipmentService
	{
		public Task<EquipmentModel> CreateEquipment(EquipmentModel equipment)
		{
			return equipmentRepository.CreateEquipment(equipment);
		}

		public Task DeleteEquipment(int id)
		{
			return equipmentRepository.DeleteEquipment(id);
		}

		public Task<bool> EquipmentModelExists(int id)
		{
			return equipmentRepository.EquipmentModelExists(id);
		}

		public Task<EquipmentModel> GetEquipment(int id)
		{
			return equipmentRepository.GetEquipment(id);
		}

		public Task<List<EquipmentModel>> GetEquipments()
		{
			return equipmentRepository.GetEquipments();
		}

		public Task UpdateEquipment(EquipmentModel equipment)
		{
			return equipmentRepository.UpdateEquipment(equipment);
		}
	}
}
