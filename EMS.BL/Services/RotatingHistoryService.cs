using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.BL.Repositories;
using EMS.Model.Entities;

namespace EMS.BL.Services
{
    public interface IRotatingHistoryService
    {
        Task<List<RotatingHistoryModel>> GetRotatingHistories();
        Task<RotatingHistoryModel> GetRotatingHistory(int id);
        Task<bool> RotatingHistoryModelExists(int id);
        Task UpdateRotatingHistory(RotatingHistoryModel rotatingHistoryModel);
        Task DeleteRotatingHistory(int id);
    }
    public class RotatingHistoryService(IRotatingHistoryRepository rotatingHistoryRepository) : IRotatingHistoryService
    {
        public Task DeleteRotatingHistory(int id)
        {
            return rotatingHistoryRepository.DeleteRotatingHistory(id);
        }

        public Task<List<RotatingHistoryModel>> GetRotatingHistories()
        {
            return rotatingHistoryRepository.GetRotatingHistories();
        }

        public Task<RotatingHistoryModel> GetRotatingHistory(int id)
        {
            return rotatingHistoryRepository.GetRotatingHistory(id);
        }

        public Task<bool> RotatingHistoryModelExists(int id)
        {
            return rotatingHistoryRepository.RotatingHistoryModelExists(id);
        }

        public Task UpdateRotatingHistory(RotatingHistoryModel rotatingHistoryModel)
        {
            return rotatingHistoryRepository.UpdateRotatingHistory(rotatingHistoryModel);
        }
    }
}
