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
	public interface IMaintenanceRequestRepository
	{
		Task<MaintenanceRequestModel> CreateRequest(MaintenanceRequestModel request);
		Task<List<MaintenanceRequestModel>> GetPendingRequestsLv2();
		Task<MaintenanceRequestModel> GetRequest(int id);
		Task<List<MaintenanceRequestModel>> GetRequestsByUserId(int id);
		Task<MaintenanceRequestModel> ApproveRequestLv2(ApproveRequestDto dto);
		Task<List<MaintenanceRequestModel>> GetPendingRequestsLv3();
		Task<MaintenanceRequestModel> ApproveRequestLv3(ApproveRequestDto dto);
		Task<MaintenanceRequestModel> CompleteApproval(int id);
		Task<MaintenanceHistoryModel> CompleteMaintenance(CompletePurchasingRequestDto dto);
		Task<List<MaintenanceRequestModel>> GetApprovedRequest();
		Task<bool> MaintenanceRequestModelExists(int id);
		Task UpdateRequest(MaintenanceRequestModel maintenanceRequest);
		Task DeleteRequest(int id);
	}
	public class MaintenanceRequestRepository(AppDbContext dbContext) : IMaintenanceRequestRepository
	{
		public async Task<MaintenanceRequestModel> ApproveRequestLv2(ApproveRequestDto dto)
		{
			var request = await dbContext.MaintenanceRequests.FindAsync(dto.RequestId);
			if (request == null)
			{
				throw new Exception("Request not found.");
			}

			request.AcceptanceLv2Status = dto.AcceptanceStatus;
			request.ReasonLv2 = dto.Reason;
			request.ReviewerLv2ID = dto.ReviewerId;

			request.ReviewerLv2 = await dbContext.Users.FindAsync(dto.ReviewerId);

			dbContext.MaintenanceRequests.Update(request);
			await dbContext.SaveChangesAsync();
			return request;
		}

		public async Task<MaintenanceRequestModel> ApproveRequestLv3(ApproveRequestDto dto)
		{
			var request = await dbContext.MaintenanceRequests.FindAsync(dto.RequestId);
			if (request == null)
			{
				throw new Exception("Request not found.");
			}
			request.AcceptanceLv3Status = dto.AcceptanceStatus;
			request.ReasonLv3 = dto.Reason;
			request.ReviewerLv3ID = dto.ReviewerId;

			request.ReviewerLv3 = await dbContext.Users.FindAsync(dto.ReviewerId);


			dbContext.MaintenanceRequests.Update(request);
			await dbContext.SaveChangesAsync();
			return request;
		}

		public async Task<MaintenanceRequestModel> CompleteApproval(int id)
		{
			var request = await dbContext.MaintenanceRequests.Include(r => r.Equipment).FirstOrDefaultAsync(r => r.ID == id);
			if (request == null)
			{
				throw new Exception("Request not found or not approved!");
			}

			if (request.AcceptanceLv2Status != true || request.AcceptanceLv3Status != true)
			{
				throw new Exception("Yêu cầu chưa được duyệt qua 2 cấp.");
			}

			// Cập nhật trạng thái bảo trì
			request.MaintenanceStatus = MaintenanceStatus.InProgress;
			request.MaintenanceStartDate = DateTime.UtcNow;

			// Cập nhật trạng thái thiết bị
			request.Equipment.Status = EquipmentStatus.DangBaoTri;

			await dbContext.SaveChangesAsync();
			return request;
		}

		public async Task<MaintenanceHistoryModel> CompleteMaintenance(CompletePurchasingRequestDto dto)
		{
			var request = await dbContext.MaintenanceRequests.Include(r => r.Equipment).FirstOrDefaultAsync(r => r.ID == dto.RequestId);
			if (request == null)
			{
				throw new Exception("Yêu cầu không tìm thấy hoặc chưa được duyệt.");
			}

			if (request.MaintenanceStatus != MaintenanceStatus.InProgress)
			{
				throw new Exception("Chỉ có thể hoàn thành bảo trì cho yêu cầu đang được bảo trì.");
			}

			// Cập nhật trạng thái bảo trì
			request.MaintenanceStatus = MaintenanceStatus.Completed;
			request.MaintenanceEndDate = DateTime.UtcNow;

			// Cập nhật trạng thái thiết bị
			request.Equipment.Status = EquipmentStatus.KhongSuDung;
			request.Equipment.ExpireDay = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(6));

			// Tạo bản ghi MaintenanceHistory
			var maintenanceHistory = new MaintenanceHistoryModel
			{
				EquipmentId = request.EquipmentId,
				MaintenanceRequestId = request.ID,
				MaintenanceStartDate = request.MaintenanceStartDate.Value,
				MaintenanceEndDate = request.MaintenanceEndDate.Value,
				ReviewerLv2ID = request.ReviewerLv2ID,
				ReviewerLv3ID = request.ReviewerLv3ID,
				Notes = dto.Note
			};

			dbContext.MaintenanceHistories.Add(maintenanceHistory);
			await dbContext.SaveChangesAsync();
			return maintenanceHistory;
		}

		public async Task<MaintenanceRequestModel> CreateRequest(MaintenanceRequestModel request)
		{
			request.AcceptanceLv2Status = null;
			request.AcceptanceLv3Status = null;
			request.ReviewerLv2ID = null;
			request.ReviewerLv3ID = null;
			request.MaintenanceStatus = MaintenanceStatus.Pending;

			request.Equipment = await dbContext.Equipments.FindAsync(request.EquipmentId);
			request.User = await dbContext.Users.FindAsync(request.UserId);

			dbContext.MaintenanceRequests.Add(request);
			await dbContext.SaveChangesAsync();
			return request;
		}

		public async Task DeleteRequest(int id)
		{
			var request = dbContext.MaintenanceRequests.FirstOrDefault(n => n.ID == id);
			dbContext.MaintenanceRequests.Remove(request);
			await dbContext.SaveChangesAsync();
		}

		public async Task<List<MaintenanceRequestModel>> GetApprovedRequest()
		{
			return await dbContext.MaintenanceRequests.Where(r => r.AcceptanceLv2Status == true && r.AcceptanceLv3Status == true).Include(r => r.User)
				.Include(r => r.Equipment)
				.Include(r => r.ReviewerLv2)
				.Include(r => r.ReviewerLv3).ToListAsync();
		}

		public async Task<List<MaintenanceRequestModel>> GetPendingRequestsLv2()
		{
			return await dbContext.MaintenanceRequests.Where(r => r.AcceptanceLv2Status == null).Include(r => r.User)
				.Include(r => r.Equipment).ToListAsync();
		}

		public async Task<List<MaintenanceRequestModel>> GetPendingRequestsLv3()
		{
			return await dbContext.MaintenanceRequests.Where(r => r.AcceptanceLv2Status == true && r.AcceptanceLv3Status == null).Include(r => r.User)
				.Include(r => r.Equipment).Include(r => r.ReviewerLv2).ToListAsync();
		}

		public async Task<MaintenanceRequestModel> GetRequest(int id)
		{
			return await dbContext.MaintenanceRequests.Include(r => r.User)
			   .Include(r => r.Equipment)
			   .Include(r => r.ReviewerLv2)
			   .Include(r => r.ReviewerLv3)
			   .FirstOrDefaultAsync(r => r.ID == id);
		}

		public async Task<List<MaintenanceRequestModel>> GetRequestsByUserId(int id)
		{
			return await dbContext.MaintenanceRequests.Where(r => r.UserId == id).Include(r => r.User)
				.Include(r => r.Equipment)
				.Include(r => r.ReviewerLv2)
				.Include(r => r.ReviewerLv3).ToListAsync();
		}

		public async Task<bool> MaintenanceRequestModelExists(int id)
		{
			return await dbContext.MaintenanceRequests.AnyAsync(e => e.ID == id);
		}

		public async Task UpdateRequest(MaintenanceRequestModel maintenanceRequest)
		{
			maintenanceRequest.Equipment = await dbContext.Equipments.FindAsync(maintenanceRequest.EquipmentId);
			maintenanceRequest.User = await dbContext.Users.FindAsync(maintenanceRequest.UserId);


			dbContext.Entry(maintenanceRequest).State = EntityState.Modified;
			await dbContext.SaveChangesAsync();
		}
	}
}
