using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.BL.Repositories;
using EMS.Model.Entities;

namespace EMS.BL.Services
{
	public interface IPurchasingHistoryService
	{
		Task<List<PurchasingHistoryModel>> GetPurchasingHistories();
		Task<PurchasingHistoryModel> GetPurchasingHistory(int id);
		Task<bool> PurchasingHistoryModelExists(int id);
		Task UpdatePurchasingHistory(PurchasingHistoryModel purchasingHistoryModel);
		Task DeletePurchasingHistory(int id);
	}
	public class PurchasingHistoryService(IPurchasingHistoryRepository purchasingHistoryRepository) : IPurchasingHistoryService
	{
		public Task DeletePurchasingHistory(int id)
		{
			return purchasingHistoryRepository.DeletePurchasingHistory(id);
		}

		public Task<List<PurchasingHistoryModel>> GetPurchasingHistories()
		{
			return purchasingHistoryRepository.GetPurchasingHistories();
		}

		public Task<PurchasingHistoryModel> GetPurchasingHistory(int id)
		{
			return purchasingHistoryRepository.GetPurchasingHistory(id);
		}

		public Task<bool> PurchasingHistoryModelExists(int id)
		{
			return purchasingHistoryRepository.PurchasingHistoryModelExists(id);
		}

		public Task UpdatePurchasingHistory(PurchasingHistoryModel purchasingHistoryModel)
		{
			return purchasingHistoryRepository.UpdatePurchasingHistory(purchasingHistoryModel);
		}
	}
}
