using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.BL.Repositories;
using EMS.Model.Entities;

namespace EMS.BL.Services
{
	public interface ILiquidationHistoryService
	{
		Task<List<LiquidationHistoryModel>> GetLiquidationHistories();
		Task<LiquidationHistoryModel> GetLiquidationHistory(int id);
		Task<bool> LiquidationHistoryModelExists(int id);
		Task UpdateLiquidationHistory(LiquidationHistoryModel liquidationHistoryModel);
		Task DeleteLiquidationHistory(int id);
	}
	public class LiquidationHistoryService(ILiquidationHistoryRepository liquidationHistoryRepository) : ILiquidationHistoryService
	{
		public Task DeleteLiquidationHistory(int id)
		{
			return liquidationHistoryRepository.DeleteLiquidationHistory(id);
		}

		public Task<List<LiquidationHistoryModel>> GetLiquidationHistories()
		{
			return liquidationHistoryRepository.GetLiquidationHistories();
		}

		public Task<LiquidationHistoryModel> GetLiquidationHistory(int id)
		{
			return liquidationHistoryRepository.GetLiquidationHistory(id);
		}

		public Task<bool> LiquidationHistoryModelExists(int id)
		{
			return liquidationHistoryRepository.LiquidationHistoryModelExists(id);
		}

		public Task UpdateLiquidationHistory(LiquidationHistoryModel liquidationHistoryModel)
		{
			return liquidationHistoryRepository.UpdateLiquidationHistory(liquidationHistoryModel);
		}
	}
}
