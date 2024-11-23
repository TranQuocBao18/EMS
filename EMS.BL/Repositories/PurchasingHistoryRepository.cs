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
	public interface IPurchasingHistoryRepository
	{
		Task<List<PurchasingHistoryModel>> GetPurchasingHistories();
		Task<PurchasingHistoryModel> GetPurchasingHistory(int id);
		Task<bool> PurchasingHistoryModelExists(int id);
		Task UpdatePurchasingHistory(PurchasingHistoryModel purchasingHistoryModel);
		Task DeletePurchasingHistory(int id);
	}
	public class PurchasingHistoryRepository(AppDbContext dbContext) : IPurchasingHistoryRepository
	{
		public async Task DeletePurchasingHistory(int id)
		{
			var history = dbContext.PurchasingHistories.FirstOrDefault(n => n.ID == id);
			dbContext.PurchasingHistories.Remove(history);
			await dbContext.SaveChangesAsync();
		}

		public async Task<List<PurchasingHistoryModel>> GetPurchasingHistories()
		{
			return await dbContext.PurchasingHistories
				.Include(r => r.ReviewerLv2)
				.Include(r => r.ReviewerLv3)
				.Include(r => r.PurchasingRequest)
				.Include(r => r.PurchasingRequestDetails).ThenInclude(r => r.EquipmentType).ToListAsync();
		}

		public async Task<PurchasingHistoryModel> GetPurchasingHistory(int id)
		{
			return await dbContext.PurchasingHistories
				.Include(r => r.ReviewerLv2)
				.Include(r => r.ReviewerLv3)
				.Include(r => r.PurchasingRequest)
				.Include(r => r.PurchasingRequestDetails).ThenInclude(r => r.EquipmentType).FirstOrDefaultAsync(n => n.ID == id);
		}

		public async Task<bool> PurchasingHistoryModelExists(int id)
		{
			return await dbContext.PurchasingHistories.AnyAsync(e => e.ID == id);

		}

		public async Task UpdatePurchasingHistory(PurchasingHistoryModel purchasingHistoryModel)
		{
			purchasingHistoryModel.ReviewerLv2 = await dbContext.Users.FindAsync(purchasingHistoryModel.ReviewerLv2ID);
			purchasingHistoryModel.ReviewerLv3 = await dbContext.Users.FindAsync(purchasingHistoryModel.ReviewerLv3ID);
			purchasingHistoryModel.PurchasingRequest = await dbContext.PurchasingRequests.FindAsync(purchasingHistoryModel.PurchasingRequestID);

			dbContext.Entry(purchasingHistoryModel).State = EntityState.Modified;
			await dbContext.SaveChangesAsync();
		}
	}
}
