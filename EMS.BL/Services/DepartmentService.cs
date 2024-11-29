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
	public interface IDepartmentService
	{
		Task<List<DepartmentModel>> GetDepartments();
		Task<int> GetDepartmentsAmount();
		Task<DepartmentModel> GetDepartment(int id);
		Task UpdateDepartment(DepartmentDto departmentDto);
		Task<DepartmentModel> CreateDepartment(DepartmentDto departmentDto);
		Task<bool> DepartmentModelExists(int id);
		Task DeleteDepartment(int id);
	}
	public class DepartmentService(IDepartmentRepository departmentRepository) : IDepartmentService
	{
		public Task<DepartmentModel> CreateDepartment(DepartmentDto departmentDto)
		{
			return departmentRepository.CreateDepartment(departmentDto);
		}

		public Task DeleteDepartment(int id)
		{
			return departmentRepository.DeleteDepartment(id);
		}

		public Task<bool> DepartmentModelExists(int id)
		{
			return departmentRepository.DepartmentModelExists(id);
		}

		public Task<DepartmentModel> GetDepartment(int id)
		{
		return departmentRepository.GetDepartment(id);
		}

		public Task<List<DepartmentModel>> GetDepartments()
		{
			return departmentRepository.GetDepartments();
		}

		public Task<int> GetDepartmentsAmount()
		{
			return departmentRepository.GetDepartmentsAmount();
		}

		public Task UpdateDepartment(DepartmentDto departmentDto)
		{
			return departmentRepository.UpdateDepartment(departmentDto);
		}
	}
}
