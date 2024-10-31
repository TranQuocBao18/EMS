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
    }
    public class RotatingHistoryService(IRotatingHistoryRepository rotatingHistoryRepository) : IRotatingHistoryService
    {
        public Task<List<RotatingHistoryModel>> GetRotatingHistories()
        {
            return rotatingHistoryRepository.GetRotatingHistories();
        }

        public Task<RotatingHistoryModel> GetRotatingHistory(int id)
        {
            return rotatingHistoryRepository.GetRotatingHistory(id);
        }
    }
}
