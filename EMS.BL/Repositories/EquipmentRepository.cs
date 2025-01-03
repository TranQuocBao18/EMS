﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Database.Data;
using EMS.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace EMS.BL.Repositories
{
	public interface IEquipmentRepository
	{
		Task<List<EquipmentModel>> GetEquipments();
		Task<List<EquipmentModel>> GetAllEquipments();
		Task<int> GetEquipmentsAmount();
		Task<EquipmentModel> GetEquipment(int id);
		Task UpdateEquipment(EquipmentModel equipment);
		Task<EquipmentModel> CreateEquipment(EquipmentModel equipment);
		Task<bool> EquipmentModelExists(int id);
		Task DeleteEquipment(int id);
	}

	public class EquipmentRepository(AppDbContext dbContext) : IEquipmentRepository
	{
		public async Task<EquipmentModel> CreateEquipment(EquipmentModel equipment)
		{
			equipment.EquipmentType = await dbContext.EquipmentTypes.FindAsync(equipment.EquipmentTypeId);
			equipment.Department = await dbContext.Departments.FindAsync(equipment.DepartmentId);
			equipment.User = await dbContext.Users.FindAsync(equipment.UserId);

			dbContext.Equipments.Add(equipment);
			await dbContext.SaveChangesAsync();
			return equipment;
		}

		public async Task DeleteEquipment(int id)
		{
			var equipment = dbContext.Equipments.FirstOrDefault(n => n.ID == id);
			dbContext.Equipments.Remove(equipment);
			await dbContext.SaveChangesAsync();
		}

		public async Task<bool> EquipmentModelExists(int id)
		{
			return await dbContext.Equipments.AnyAsync(e => e.ID == id);
		}

		public async Task<List<EquipmentModel>> GetAllEquipments()
		{
			return await dbContext.Equipments.Include(e => e.EquipmentType).Include(e => e.Department).Include(e => e.User).ToListAsync();
		}

		public async Task<EquipmentModel> GetEquipment(int id)
		{
			return await dbContext.Equipments.Include(n => n.EquipmentType).Include(n => n.Department).Include(n => n.User).FirstOrDefaultAsync(n => n.ID == id);
		}

		public async Task<List<EquipmentModel>> GetEquipments()
		{
			return await dbContext.Equipments.Include(e => e.EquipmentType).Include(e => e.Department).Include(e => e.User).Where(e => e.Status != EquipmentStatus.DaThanhLy).ToListAsync();
		}

		public async Task<int> GetEquipmentsAmount()
		{
			return await dbContext.Equipments.CountAsync();

		}

		public async Task UpdateEquipment(EquipmentModel equipment)
		{
			equipment.EquipmentType = await dbContext.EquipmentTypes.FindAsync(equipment.EquipmentTypeId);
			equipment.Department = await dbContext.Departments.FindAsync(equipment.DepartmentId);
			equipment.User = await dbContext.Users.FindAsync(equipment.UserId);

			dbContext.Entry(equipment).State = EntityState.Modified;
			await dbContext.SaveChangesAsync();
		}
	}
}
