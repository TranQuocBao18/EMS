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
	public interface IRoleRepository
	{
		Task<List<RoleModel>> GetRoles();
	}
	public class RoleRepository(AppDbContext dbContext) :IRoleRepository
	{
		public async Task<List<RoleModel>> GetRoles()
		{
			return await dbContext.Roles.ToListAsync();
		}
	}
}
