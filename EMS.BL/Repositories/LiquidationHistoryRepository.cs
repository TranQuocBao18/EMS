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
	public interface ILiquidationHistoryRepository
	{
		Task<List<LiquidationHistoryModel>> GetLiquidationHistories();
		Task<LiquidationHistoryModel> GetLiquidationHistory(int id);
		Task<bool> LiquidationHistoryModelExists(int id);
		Task UpdateLiquidationHistory(LiquidationHistoryModel liquidationHistoryModel);
		Task DeleteLiquidationHistory(int id);
	}
	public class LiquidationHistoryRepository(AppDbContext dbContext) : ILiquidationHistoryRepository
	{
		public async Task DeleteLiquidationHistory(int id)
		{
			var history = dbContext.LiquidationHistories.FirstOrDefault(n => n.ID == id);
			dbContext.LiquidationHistories.Remove(history);
			await dbContext.SaveChangesAsync();
		}

		public async Task<List<LiquidationHistoryModel>> GetLiquidationHistories()
		{
			return await dbContext.LiquidationHistories.Include(r => r.LiquidationRequest)
				.Include(r => r.Equipment)
				.Include(r => r.ReviewerLv2)
				.Include(r => r.ReviewerLv3).ToListAsync();
		}

		public async Task<LiquidationHistoryModel> GetLiquidationHistory(int id)
		{
			return await dbContext.LiquidationHistories.Include(r => r.LiquidationRequest)
				.Include(r => r.Equipment)
				.Include(r => r.ReviewerLv2)
				.Include(r => r.ReviewerLv3).FirstOrDefaultAsync(n => n.ID == id);
		}

		public async Task<bool> LiquidationHistoryModelExists(int id)
		{
			return await dbContext.LiquidationHistories.AnyAsync(e => e.ID == id);
		}

		public async Task UpdateLiquidationHistory(LiquidationHistoryModel liquidationHistoryModel)
		{
			liquidationHistoryModel.Equipment = await dbContext.Equipments.FindAsync(liquidationHistoryModel.EquipmentId);
			liquidationHistoryModel.ReviewerLv2 = await dbContext.Users.FindAsync(liquidationHistoryModel.ReviewerLv2ID);
			liquidationHistoryModel.ReviewerLv3 = await dbContext.Users.FindAsync(liquidationHistoryModel.ReviewerLv3ID);
			liquidationHistoryModel.LiquidationRequest = await dbContext.LiquidationRequests.FindAsync(liquidationHistoryModel.LiquidationRequestID);


			dbContext.Entry(liquidationHistoryModel).State = EntityState.Modified;
			await dbContext.SaveChangesAsync();
		}
	}
}
