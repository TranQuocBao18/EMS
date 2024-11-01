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
        Task<bool> RotatingHistoryModelExists(int id);
        Task UpdateRotatingHistory(RotatingHistoryModel rotatingHistoryModel);
        Task DeleteRotatingHistory(int id);
    }
    public class RotatingHistoryRepository(AppDbContext dbContext) : IRotatingHistoryRepository
    {
        public async Task DeleteRotatingHistory(int id)
        {
            var history = dbContext.RotatingHistories.FirstOrDefault(n => n.ID == id);
            dbContext.RotatingHistories.Remove(history);
            await dbContext.SaveChangesAsync();
        }

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

        public async Task<bool> RotatingHistoryModelExists(int id)
        {
            return await dbContext.RotatingHistories.AnyAsync(e => e.ID == id);
        }

        public async Task UpdateRotatingHistory(RotatingHistoryModel rotatingHistoryModel)
        {
            rotatingHistoryModel.RequestUser = await dbContext.Users.FindAsync(rotatingHistoryModel.RequestUserId);
            rotatingHistoryModel.Equipment = await dbContext.Equipments.FindAsync(rotatingHistoryModel.EquipmentId);
            rotatingHistoryModel.FromDepartment = await dbContext.Departments.FindAsync(rotatingHistoryModel.FromDepartmentId);
            rotatingHistoryModel.ToDepartment = await dbContext.Departments.FindAsync(rotatingHistoryModel.ToDepartmentId);
            rotatingHistoryModel.ReviewerLv2 = await dbContext.Users.FindAsync(rotatingHistoryModel.ReviewerLv2Id);
            rotatingHistoryModel.ReviewerLv3 = await dbContext.Users.FindAsync(rotatingHistoryModel.ReviewerLv3Id);


            dbContext.Entry(rotatingHistoryModel).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
    }
}
