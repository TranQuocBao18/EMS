using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Database.Data;
using EMS.Model.DTOs;
using EMS.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace EMS.BL.Repositories
{
	public interface IDepartmentRepository
	{
		Task<List<DepartmentModel>> GetDepartments();
		Task<DepartmentModel> GetDepartment(int id);
		Task UpdateDepartment(DepartmentDTO departmentDTO);
		Task<DepartmentModel> CreateDepartment(DepartmentDTO departmentDTO);
		Task<bool> DepartmentModelExists(int id);
		Task DeleteDepartment(int id);
	}
	public class DepartmentRepository(AppDbContext dbContext) : IDepartmentRepository
	{
		public async Task<DepartmentModel> CreateDepartment(DepartmentDTO departmentDTO)
		{
			// Tạo một đối tượng DepartmentModel từ DepartmentDTO
			var departmentModel = new DepartmentModel
			{
				DepartmentName = departmentDTO.DepartmentName
			};
			dbContext.Departments.Add(departmentModel);
			await dbContext.SaveChangesAsync();
			return departmentModel;
		}

		public async Task DeleteDepartment(int id)
		{
			var department = dbContext.Departments.FirstOrDefault(n => n.ID == id);
			dbContext.Departments.Remove(department);
			await dbContext.SaveChangesAsync();
		}

		public Task<bool> DepartmentModelExists(int id)
		{
			return dbContext.Departments.AnyAsync(e => e.ID == id);
		}

		public Task<DepartmentModel> GetDepartment(int id)
		{
			return dbContext.Departments.FirstOrDefaultAsync(n => n.ID == id);
		}

		public Task<List<DepartmentModel>> GetDepartments()
		{
			return dbContext.Departments.ToListAsync();
		}

		public async Task UpdateDepartment(DepartmentDTO departmentDTO)
		{
			var departmentModel = await dbContext.Departments.FindAsync(departmentDTO.ID);
			if (departmentModel == null)
			{
				throw new Exception("Department not found.");
			}

			departmentModel.DepartmentName = departmentDTO.DepartmentName;

			dbContext.Entry(departmentModel).State = EntityState.Modified;
			await dbContext.SaveChangesAsync();
		}
	}
}
