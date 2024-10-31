using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Database.Data;
using EMS.Model.DTOs;
using EMS.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace EMS.BL.Repositories
{
    public interface IRotatingRequestRepository
    {
        Task<RotatingRequestModel> CreateRequest(RotatingRequestModel request);
        Task<List<RotatingRequestModel>> GetPendingRequestsLv2();
        Task<RotatingRequestModel> GetRequest(int id);
        Task<List<RotatingRequestModel>> GetRequestsByUserId(int id);
        Task<RotatingRequestModel> ApproveRequestLv2(ApproveRequestDto dto);
        Task<List<RotatingRequestModel>> GetPendingRequestsLv3();
        Task<RotatingRequestModel> ApproveRequestLv3(ApproveRequestDto dto);
        Task<RotatingHistoryModel> CompleteRequest(CompleteRequestDto dto);
        Task<List<RotatingRequestModel>> GetApprovedRequest();
        Task<bool> RotatingRequestModelExists(int id);
        Task UpdateRequest(RotatingRequestModel rotatingRequestModel);
        Task DeleteRequest(int id);

    }
    public class RotatingRequestRepository(AppDbContext dbContext) : IRotatingRequestRepository
    {
        public async Task<RotatingRequestModel> ApproveRequestLv2(ApproveRequestDto dto)
        {
            var request = await dbContext.RotatingRequests.FindAsync(dto.RequestId);
            if (request == null)
            {
                throw new Exception("Request not found.");
            }

            request.AcceptanceLv2Status = dto.AcceptanceStatus;
            request.ReasonLv2 = dto.Reason;
            request.ReviewerLv2ID = dto.ReviewerId;

            request.ReviewerLv2 = await dbContext.Users.FindAsync(dto.ReviewerId);

            dbContext.RotatingRequests.Update(request);
            await dbContext.SaveChangesAsync();
            return request;
        }

        public async Task<RotatingRequestModel> ApproveRequestLv3(ApproveRequestDto dto)
        {
            var request = await dbContext.RotatingRequests.FindAsync(dto.RequestId);
            if (request == null)
            {
                throw new Exception("Request not found.");
            }
            request.AcceptanceLv3Status = dto.AcceptanceStatus;
            request.ReasonLv3 = dto.Reason;
            request.ReviewerLv3ID = dto.ReviewerId;

            request.ReviewerLv3 = await dbContext.Users.FindAsync(dto.ReviewerId);


            dbContext.RotatingRequests.Update(request);
            await dbContext.SaveChangesAsync();
            return request;
        }

        public async Task<RotatingHistoryModel> CompleteRequest(CompleteRequestDto dto)
        {
            var request = await dbContext.RotatingRequests.FindAsync(dto.RequestId);
            if (request == null && request.AcceptanceLv2Status != true && request.AcceptanceLv3Status != true)
            {
                throw new Exception("Request not found or not approved!");
            }


            var equipment = await dbContext.Equipments.FindAsync(dto.EquipmentId);
            if (equipment == null)
            {
                throw new Exception("Equipment not found.");
            }

            equipment.DepartmentId = dto.NewDepartmentId;
            equipment.Status = EquipmentStatus.DangSuDung;

            var history = new RotatingHistoryModel
            {
                RequestUserId = request.UserId,
                EquipmentId = dto.EquipmentId,
                RotatingDate = DateTime.Now,
                FromDepartmentId = dto.OldDepartmentId,
                ToDepartmentId = dto.NewDepartmentId,
                ReviewerLv2Id = request.ReviewerLv2ID,
                ReviewerLv3Id = request.ReviewerLv3ID
            };

            dbContext.Equipments.Update(equipment);
            dbContext.RotatingHistories.Add(history);

            dbContext.RotatingRequests.Remove(request);
            await dbContext.SaveChangesAsync();

            return history;
        }

        public async Task<RotatingRequestModel> CreateRequest(RotatingRequestModel request)
        {
            request.AcceptanceLv2Status = null;
            request.AcceptanceLv3Status = null;
            request.ReviewerLv2ID = null;
            request.ReviewerLv3ID = null;

            request.Department = await dbContext.Departments.FindAsync(request.DepartmentId);
            request.EquipmentType = await dbContext.EquipmentTypes.FindAsync(request.EquipmentTypeId);
            request.User = await dbContext.Users.FindAsync(request.UserId);

            dbContext.RotatingRequests.Add(request);
            await dbContext.SaveChangesAsync();
            return request;
        }

        public async Task<RotatingRequestModel> GetRequest(int id)
        {
            return await dbContext.RotatingRequests.Include(r => r.User)
                .Include(r => r.EquipmentType)
                .Include(r => r.Department)
                .Include(r => r.ReviewerLv2)
                .Include(r => r.ReviewerLv3)
                .FirstOrDefaultAsync(r => r.ID == id);
        }

        public async Task<List<RotatingRequestModel>> GetApprovedRequest()
        {
            return await dbContext.RotatingRequests.Where(r => r.AcceptanceLv2Status == true && r.AcceptanceLv3Status == true).Include(r => r.User)
                .Include(r => r.EquipmentType)
                .Include(r => r.Department)
                .Include(r => r.ReviewerLv2)
                .Include(r => r.ReviewerLv3).ToListAsync();
        }

        public async Task<List<RotatingRequestModel>> GetPendingRequestsLv2()
        {
            return await dbContext.RotatingRequests.Where(r => r.AcceptanceLv2Status == null).Include(r => r.User)
                .Include(r => r.EquipmentType)
                .Include(r => r.Department).ToListAsync();
        }

        public async Task<List<RotatingRequestModel>> GetPendingRequestsLv3()
        {
            return await dbContext.RotatingRequests.Where(r => r.AcceptanceLv2Status == true && r.AcceptanceLv3Status == null).Include(r => r.User)
                .Include(r => r.EquipmentType)
                .Include(r => r.Department)
                .Include(r => r.ReviewerLv2).ToListAsync();
        }

        public async Task<List<RotatingRequestModel>> GetRequestsByUserId(int id)
        {
            return await dbContext.RotatingRequests.Where(r => r.UserId == id).Include(r => r.User)
                .Include(r => r.EquipmentType)
                .Include(r => r.Department)
                .Include(r => r.ReviewerLv2)
                .Include(r => r.ReviewerLv3).ToListAsync();
        }

        public async Task<bool> RotatingRequestModelExists(int id)
        {
            return await dbContext.RotatingRequests.AnyAsync(e => e.ID == id);
        }

        public async Task DeleteRequest(int id)
        {
            var request = dbContext.RotatingRequests.FirstOrDefault(n => n.ID == id);
            dbContext.RotatingRequests.Remove(request);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateRequest(RotatingRequestModel rotatingRequestModel)
        {
            rotatingRequestModel.EquipmentType = await dbContext.EquipmentTypes.FindAsync(rotatingRequestModel.EquipmentTypeId);
            rotatingRequestModel.Department = await dbContext.Departments.FindAsync(rotatingRequestModel.DepartmentId);
            rotatingRequestModel.User = await dbContext.Users.FindAsync(rotatingRequestModel.UserId);


            dbContext.Entry(rotatingRequestModel).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
    }
}
