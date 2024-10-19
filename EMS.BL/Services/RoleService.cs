using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.BL.Repositories;
using EMS.Model.Entities;

namespace EMS.BL.Services
{
	public interface IRoleService
	{
		Task<List<RoleModel>> GetRoles();
	}
	public class RoleService(IRoleRepository roleRepository) : IRoleService
	{
		public Task<List<RoleModel>> GetRoles()
		{
			return roleRepository.GetRoles();
		}
	}
}
