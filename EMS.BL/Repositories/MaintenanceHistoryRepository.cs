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
	public interface IMaintenanceHistoryRepository
	{
		Task<List<MaintenanceHistoryModel>> GetMaintenanceHistories();
		Task<MaintenanceHistoryModel> GetMaintenanceHistory(int id);
		Task<bool> MaintenanceHistoryModelExists(int id);
		Task UpdateMaintenanceHistory(MaintenanceHistoryModel maintenanceHistoryModel);
		Task DeleteMaintenanceHistory(int id);
	}
	public class MaintenanceHistoryRepository(AppDbContext dbContext) : IMaintenanceHistoryRepository
	{
		public async Task DeleteMaintenanceHistory(int id)
		{
			var history = dbContext.MaintenanceHistories.FirstOrDefault(n => n.ID == id);
			dbContext.MaintenanceHistories.Remove(history);
			await dbContext.SaveChangesAsync();
		}

		public async Task<List<MaintenanceHistoryModel>> GetMaintenanceHistories()
		{
			return await dbContext.MaintenanceHistories
				.Include(r => r.Equipment)
				.Include(r => r.ReviewerLv2)
				.Include(r => r.ReviewerLv3)
				.Include(r => r.MaintenanceRequest).ToListAsync();
		}

		public async Task<MaintenanceHistoryModel> GetMaintenanceHistory(int id)
		{
			return await dbContext.MaintenanceHistories
				.Include(r => r.Equipment)
				.Include(r => r.ReviewerLv2)
				.Include(r => r.ReviewerLv3)
				.Include(r => r.MaintenanceRequest).FirstOrDefaultAsync(n => n.ID == id);
		}

		public async Task<bool> MaintenanceHistoryModelExists(int id)
		{
			return await dbContext.MaintenanceHistories.AnyAsync(e => e.ID == id);
		}

		public async Task UpdateMaintenanceHistory(MaintenanceHistoryModel maintenanceHistoryModel)
		{
			maintenanceHistoryModel.Equipment = await dbContext.Equipments.FindAsync(maintenanceHistoryModel.EquipmentId);
			maintenanceHistoryModel.ReviewerLv2 = await dbContext.Users.FindAsync(maintenanceHistoryModel.ReviewerLv2ID);
			maintenanceHistoryModel.ReviewerLv3 = await dbContext.Users.FindAsync(maintenanceHistoryModel.ReviewerLv3ID);
			maintenanceHistoryModel.MaintenanceRequest = await dbContext.MaintenanceRequests.FindAsync(maintenanceHistoryModel.MaintenanceRequestId);


			dbContext.Entry(maintenanceHistoryModel).State = EntityState.Modified;
			await dbContext.SaveChangesAsync();
		}
	}
}
