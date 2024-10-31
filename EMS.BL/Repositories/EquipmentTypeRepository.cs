using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Database.Data;
using EMS.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace EMS.BL.Repositories
{
	public interface IEquipmentTypeRepository
	{
		Task<List<EquipmentTypeModel>> GetEquipmentTypes();
		Task<EquipmentTypeModel> GetEquipmentType(int id);
		Task UpdateEquipmentType(EquipmentTypeModel equipmentType);
		Task<EquipmentTypeModel> CreateEquipmentType(EquipmentTypeModel equipmentType);
		Task<bool> EquipmentTypeModelExists(int id);
		Task DeleteEquipmentType(int id);
	}

	public class EquipmentTypeRepository(AppDbContext dbContext) : IEquipmentTypeRepository
	{
		public async Task<EquipmentTypeModel> CreateEquipmentType(EquipmentTypeModel equipmentType)
		{
			dbContext.EquipmentTypes.Add(equipmentType);
			await dbContext.SaveChangesAsync();
			return equipmentType;
		}

		public async Task DeleteEquipmentType(int id)
		{
			var equipmentType = dbContext.EquipmentTypes.FirstOrDefault(n => n.ID == id);
			dbContext.EquipmentTypes.Remove(equipmentType);
			await dbContext.SaveChangesAsync();
		}

		public async Task<bool> EquipmentTypeModelExists(int id)
		{
			return await dbContext.EquipmentTypes.AnyAsync(e => e.ID == id);
		}

		public async Task<EquipmentTypeModel> GetEquipmentType(int id)
		{
			return await dbContext.EquipmentTypes.FirstOrDefaultAsync(n => n.ID == id);
		}

		public async Task<List<EquipmentTypeModel>> GetEquipmentTypes()
		{
			return await dbContext.EquipmentTypes.ToListAsync();
		}

		public async Task UpdateEquipmentType(EquipmentTypeModel equipmentType)
		{
			dbContext.Entry(equipmentType).State = EntityState.Modified;
			await dbContext.SaveChangesAsync();
		}
	}
}
