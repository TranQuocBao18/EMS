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
	public interface IPurchasingRequestRepository
	{
		Task<PurchasingRequestModel> CreateRequest(PurchasingRequestModel request);
		Task<List<PurchasingRequestModel>> GetPendingRequestsLv2();
		Task<PurchasingRequestModel> GetRequest(int id);
		Task<List<PurchasingRequestModel>> GetRequestsByUserId(int id);
		Task<PurchasingRequestModel> ApproveRequestLv2(ApproveRequestDto dto);
		Task<List<PurchasingRequestModel>> GetPendingRequestsLv3();
		Task<PurchasingRequestModel> ApproveRequestLv3(ApproveRequestDto dto);
		Task<RotatingHistoryModel> CompleteRequest(CompleteRequestDto dto);
		Task<List<PurchasingRequestModel>> GetApprovedRequest();
		Task<bool> PurchasingRequestModelExists(int id);
		Task UpdateRequest(PurchasingRequestModel PurchasingRequestModel);
		Task DeleteRequest(int id);
	}
	public class PurchasingRequestRepository(AppDbContext dbContext) : IPurchasingRequestRepository
	{
		public async Task<PurchasingRequestModel> ApproveRequestLv2(ApproveRequestDto dto)
		{
			var request = await dbContext.PurchasingRequests.FindAsync(dto.RequestId);
			if (request == null)
			{
				throw new Exception("Request not found.");
			}

			request.AcceptanceLv2Status = dto.AcceptanceStatus;
			request.ReasonLv2 = dto.Reason;
			request.ReviewerLv2ID = dto.ReviewerId;

			request.ReviewerLv2 = await dbContext.Users.FindAsync(dto.ReviewerId);

			dbContext.PurchasingRequests.Update(request);
			await dbContext.SaveChangesAsync();
			return request;
		}

		public async Task<PurchasingRequestModel> ApproveRequestLv3(ApproveRequestDto dto)
		{
			var request = await dbContext.PurchasingRequests.FindAsync(dto.RequestId);
			if (request == null)
			{
				throw new Exception("Request not found.");
			}
			request.AcceptanceLv3Status = dto.AcceptanceStatus;
			request.ReasonLv3 = dto.Reason;
			request.ReviewerLv3ID = dto.ReviewerId;

			request.ReviewerLv3 = await dbContext.Users.FindAsync(dto.ReviewerId);


			dbContext.PurchasingRequests.Update(request);
			await dbContext.SaveChangesAsync();
			return request;
		}

		public Task<RotatingHistoryModel> CompleteRequest(CompleteRequestDto dto)
		{
			throw new NotImplementedException();
		}

		public async Task<PurchasingRequestModel> CreateRequest(PurchasingRequestModel request)
		{
			request.AcceptanceLv2Status = null;
			request.AcceptanceLv3Status = null;
			request.ReviewerLv2ID = null;
			request.ReviewerLv3ID = null;

			
			request.User = await dbContext.Users.FindAsync(request.UserId);

			dbContext.PurchasingRequests.Add(request);
			await dbContext.SaveChangesAsync();
			return request;
		}

		public async Task DeleteRequest(int id)
		{
			var request = dbContext.PurchasingRequests.FirstOrDefault(n => n.ID == id);
			dbContext.PurchasingRequests.Remove(request);
			await dbContext.SaveChangesAsync();
		}

		public async Task<List<PurchasingRequestModel>> GetApprovedRequest()
		{
			return await dbContext.PurchasingRequests.Where(r => r.AcceptanceLv2Status == true && r.AcceptanceLv3Status == true).Include(r => r.User)
				.Include(r => r.PurchasingRequestDetails)
				.Include(r => r.ReviewerLv2)
				.Include(r => r.ReviewerLv3)
				.ToListAsync();
		}

		public async Task<List<PurchasingRequestModel>> GetPendingRequestsLv2()
		{
			return await dbContext.PurchasingRequests.Where(r => r.AcceptanceLv2Status == null).Include(r => r.User)
				.Include(r => r.PurchasingRequestDetails).ToListAsync();
		}

		public async Task<List<PurchasingRequestModel>> GetPendingRequestsLv3()
		{
			return await dbContext.PurchasingRequests.Where(r => r.AcceptanceLv2Status == true && r.AcceptanceLv3Status == null).Include(r => r.User)
				.Include(r => r.PurchasingRequestDetails).Include(r => r.ReviewerLv2).ToListAsync();
		}

		public async Task<PurchasingRequestModel> GetRequest(int id)
		{
			return await dbContext.PurchasingRequests.Include(r => r.User)
				.Include(r => r.PurchasingRequestDetails)
				.Include(r => r.ReviewerLv2)
				.Include(r => r.ReviewerLv3)
				.FirstOrDefaultAsync(r => r.ID == id);
		}

		public async Task<List<PurchasingRequestModel>> GetRequestsByUserId(int id)
		{
			return await dbContext.PurchasingRequests.Where(r => r.UserId == id).Include(r => r.User)
				.Include(r => r.PurchasingRequestDetails)
				.Include(r => r.ReviewerLv2)
				.Include(r => r.ReviewerLv3)
				.ToListAsync();
		}

		public async Task<bool> PurchasingRequestModelExists(int id)
		{
			return await dbContext.PurchasingRequests.AnyAsync(e => e.ID == id);
		}

		public async Task UpdateRequest(PurchasingRequestModel PurchasingRequestModel)
		{
			PurchasingRequestModel.User = await dbContext.Users.FindAsync(PurchasingRequestModel.UserId);


			dbContext.Entry(PurchasingRequestModel).State = EntityState.Modified;
			await dbContext.SaveChangesAsync();
		}
	}
}
