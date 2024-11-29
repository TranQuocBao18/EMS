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
	public interface ILiquidationRequestRepository
	{
		Task<LiquidationRequestModel> CreateRequest(LiquidationRequestModel request);
		Task<List<LiquidationRequestModel>> GetPendingRequestsLv2();
		Task<LiquidationRequestModel> GetRequest(int id);
		Task<List<LiquidationRequestModel>> GetRequestsByUserId(int id);
		Task<LiquidationRequestModel> ApproveRequestLv2(ApproveRequestDto dto);
		Task<List<LiquidationRequestModel>> GetPendingRequestsLv3();
		Task<LiquidationRequestModel> ApproveRequestLv3(ApproveRequestDto dto);
		Task<LiquidationHistoryModel> CompleteRequest(CompletePurchasingRequestDto dto);
		Task<List<LiquidationRequestModel>> GetApprovedRequest();
		Task<bool> LiquidationRequestModelExists(int id);
		Task UpdateRequest(LiquidationRequestModel liquidationRequestModel);
		Task DeleteRequest(int id);
	}
	public class LiquidationRequestRepository(AppDbContext dbContext) : ILiquidationRequestRepository
	{
		public async Task<LiquidationRequestModel> ApproveRequestLv2(ApproveRequestDto dto)
		{
			var request = await dbContext.LiquidationRequests.FindAsync(dto.RequestId);
			if (request == null)
			{
				throw new Exception("Request not found.");
			}

			request.AcceptanceLv2Status = dto.AcceptanceStatus;
			request.ReasonLv2 = dto.Reason;
			request.ReviewerLv2ID = dto.ReviewerId;

			request.ReviewerLv2 = await dbContext.Users.FindAsync(dto.ReviewerId);

			dbContext.LiquidationRequests.Update(request);
			await dbContext.SaveChangesAsync();
			return request;
		}

		public async Task<LiquidationRequestModel> ApproveRequestLv3(ApproveRequestDto dto)
		{
			var request = await dbContext.LiquidationRequests.FindAsync(dto.RequestId);
			if (request == null)
			{
				throw new Exception("Request not found.");
			}
			request.AcceptanceLv3Status = dto.AcceptanceStatus;
			request.ReasonLv3 = dto.Reason;
			request.ReviewerLv3ID = dto.ReviewerId;

			request.ReviewerLv3 = await dbContext.Users.FindAsync(dto.ReviewerId);


			dbContext.LiquidationRequests.Update(request);
			await dbContext.SaveChangesAsync();
			return request;
		}

		public async Task<LiquidationHistoryModel> CompleteRequest(CompletePurchasingRequestDto dto)
		{
			var request = await dbContext.LiquidationRequests.FindAsync(dto.RequestId);
			if (request == null || request != null && request.AcceptanceLv2Status != true && request.AcceptanceLv3Status != true || request != null && request.AcceptanceLv2Status == true && request.AcceptanceLv3Status != true)
			{
				throw new Exception("Request not found or not approved!");
			}


			var equipment = await dbContext.Equipments.FindAsync(request.EquipmentId);
			if (equipment == null)
			{
				throw new Exception("Equipment not found.");
			}

			var history = new LiquidationHistoryModel
			{
				EquipmentId = request.EquipmentId,
				LiquidDate = DateTime.Now,
				ReviewerLv2ID = request.ReviewerLv2ID,
				ReviewerLv3ID = request.ReviewerLv3ID,
				LiquidationRequestID = request.ID,
				Notes = dto.Note
			};
			equipment.Status = EquipmentStatus.DaThanhLy;
			equipment.ExpireDay = null;

			dbContext.LiquidationHistories.Add(history);
			await dbContext.SaveChangesAsync();

			return history;
		}

		public async Task<LiquidationRequestModel> CreateRequest(LiquidationRequestModel request)
		{
			request.AcceptanceLv2Status = null;
			request.AcceptanceLv3Status = null;
			request.ReviewerLv2ID = null;
			request.ReviewerLv3ID = null;

			request.Equipment = await dbContext.Equipments.FindAsync(request.EquipmentId);
			request.User = await dbContext.Users.FindAsync(request.UserId);

			dbContext.LiquidationRequests.Add(request);
			await dbContext.SaveChangesAsync();
			return request;
		}

		public async Task DeleteRequest(int id)
		{
			var request = dbContext.LiquidationRequests.FirstOrDefault(n => n.ID == id);
			dbContext.LiquidationRequests.Remove(request);
			await dbContext.SaveChangesAsync();
		}

		public async Task<List<LiquidationRequestModel>> GetApprovedRequest()
		{
			return await dbContext.LiquidationRequests.Where(r => r.AcceptanceLv2Status == true && r.AcceptanceLv3Status == true).Include(r => r.User)
				.Include(r => r.Equipment)
				.Include(r => r.ReviewerLv2)
				.Include(r => r.ReviewerLv3).ToListAsync();
		}

		public async Task<List<LiquidationRequestModel>> GetPendingRequestsLv2()
		{
			return await dbContext.LiquidationRequests.Where(r => r.AcceptanceLv2Status == null).Include(r => r.User)
				.Include(r => r.Equipment).ToListAsync();
		}

		public async Task<List<LiquidationRequestModel>> GetPendingRequestsLv3()
		{
			return await dbContext.LiquidationRequests.Where(r => r.AcceptanceLv2Status == true && r.AcceptanceLv3Status == null).Include(r => r.User)
				.Include(r => r.Equipment)
				.Include(r => r.ReviewerLv2).ToListAsync();
		}

		public async Task<LiquidationRequestModel> GetRequest(int id)
		{
			return await dbContext.LiquidationRequests.Include(r => r.User)
				.Include(r => r.Equipment)
				.Include(r => r.ReviewerLv2)
				.Include(r => r.ReviewerLv3)
				.FirstOrDefaultAsync(r => r.ID == id);
		}

		public async Task<List<LiquidationRequestModel>> GetRequestsByUserId(int id)
		{
			return await dbContext.LiquidationRequests.Where(r => r.UserId == id).Include(r => r.User)
				.Include(r => r.Equipment)
				.Include(r => r.ReviewerLv2)
				.Include(r => r.ReviewerLv3).ToListAsync();
		}

		public async Task<bool> LiquidationRequestModelExists(int id)
		{
			return await dbContext.LiquidationRequests.AnyAsync(e => e.ID == id);

		}

		public async Task UpdateRequest(LiquidationRequestModel liquidationRequestModel)
		{
			liquidationRequestModel.Equipment = await dbContext.Equipments.FindAsync(liquidationRequestModel.EquipmentId);
			liquidationRequestModel.User = await dbContext.Users.FindAsync(liquidationRequestModel.UserId);


			dbContext.Entry(liquidationRequestModel).State = EntityState.Modified;
			await dbContext.SaveChangesAsync();
		}
	}
}
