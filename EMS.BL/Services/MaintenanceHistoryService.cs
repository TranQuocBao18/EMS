using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.BL.Repositories;
using EMS.Model.Entities;

namespace EMS.BL.Services
{
    public interface IMaintenanceHistoryService
    {
		Task<List<MaintenanceHistoryModel>> GetMaintenanceHistories();
		Task<MaintenanceHistoryModel> GetMaintenanceHistory(int id);
		Task<bool> MaintenanceHistoryModelExists(int id);
		Task UpdateMaintenanceHistory(MaintenanceHistoryModel maintenanceHistoryModel);
		Task DeleteMaintenanceHistory(int id);
	}

	public class MaintenanceHistoryService(IMaintenanceHistoryRepository maintenanceHistoryRepository) : IMaintenanceHistoryService
	{
		public Task DeleteMaintenanceHistory(int id)
		{
			return maintenanceHistoryRepository.DeleteMaintenanceHistory(id);
		}

		public Task<List<MaintenanceHistoryModel>> GetMaintenanceHistories()
		{
			return maintenanceHistoryRepository.GetMaintenanceHistories();
		}

		public Task<MaintenanceHistoryModel> GetMaintenanceHistory(int id)
		{
			return maintenanceHistoryRepository.GetMaintenanceHistory(id);
		}

		public Task<bool> MaintenanceHistoryModelExists(int id)
		{
			return maintenanceHistoryRepository.MaintenanceHistoryModelExists(id);
		}

		public Task UpdateMaintenanceHistory(MaintenanceHistoryModel maintenanceHistoryModel)
		{
			return maintenanceHistoryRepository.UpdateMaintenanceHistory(maintenanceHistoryModel);
		}
	}
}
