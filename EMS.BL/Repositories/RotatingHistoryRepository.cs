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
    public interface IRotatingHistoryRepository
    {
        Task<List<RotatingHistoryModel>> GetRotatingHistories();
        Task<RotatingHistoryModel> GetRotatingHistory(int id);
    }
    public class RotatingHistoryRepository(AppDbContext dbContext) : IRotatingHistoryRepository
    {
        public async Task<List<RotatingHistoryModel>> GetRotatingHistories()
        {
            return await dbContext.RotatingHistories.Include(r => r.RequestUser)
                .Include(r => r.Equipment)
                .Include(r => r.FromDepartment)
                .Include(r => r.ToDepartment)
                .Include(r => r.ReviewerLv2)
                .Include(r => r.ReviewerLv3).ToListAsync();
        }

        public async Task<RotatingHistoryModel> GetRotatingHistory(int id)
        {
            return await dbContext.RotatingHistories.Include(r => r.RequestUser)
                .Include(r => r.Equipment)
                .Include(r => r.FromDepartment)
                .Include(r => r.ToDepartment)
                .Include(r => r.ReviewerLv2)
                .Include(r => r.ReviewerLv3).FirstOrDefaultAsync(n => n.ID == id);
        }
    }
}
