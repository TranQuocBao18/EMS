using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.BL.Repositories;
using EMS.Model.DTOs;
using EMS.Model.Entities;

namespace EMS.BL.Services
{
    public interface ILiquidationRequestService
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

	public class LiquidationRequestService(ILiquidationRequestRepository liquidationRequestRepository) : ILiquidationRequestService
	{
		public Task<LiquidationRequestModel> ApproveRequestLv2(ApproveRequestDto dto)
		{
			return liquidationRequestRepository.ApproveRequestLv2(dto);
		}

		public Task<LiquidationRequestModel> ApproveRequestLv3(ApproveRequestDto dto)
		{
			return liquidationRequestRepository.ApproveRequestLv3(dto);
		}

		public Task<LiquidationHistoryModel> CompleteRequest(CompletePurchasingRequestDto dto)
		{
			return liquidationRequestRepository.CompleteRequest(dto);
		}

		public Task<LiquidationRequestModel> CreateRequest(LiquidationRequestModel request)
		{
			return liquidationRequestRepository.CreateRequest(request);
		}

		public Task DeleteRequest(int id)
		{
			return liquidationRequestRepository.DeleteRequest(id);
		}

		public Task<List<LiquidationRequestModel>> GetApprovedRequest()
		{
			return liquidationRequestRepository.GetApprovedRequest();
		}

		public Task<List<LiquidationRequestModel>> GetPendingRequestsLv2()
		{
			return liquidationRequestRepository.GetPendingRequestsLv2();
		}

		public Task<List<LiquidationRequestModel>> GetPendingRequestsLv3()
		{
			return liquidationRequestRepository.GetPendingRequestsLv3();
		}

		public Task<LiquidationRequestModel> GetRequest(int id)
		{
			return liquidationRequestRepository.GetRequest(id);
		}

		public Task<List<LiquidationRequestModel>> GetRequestsByUserId(int id)
		{
			return liquidationRequestRepository.GetRequestsByUserId(id);
		}

		public Task<bool> LiquidationRequestModelExists(int id)
		{
			return liquidationRequestRepository.LiquidationRequestModelExists(id);
		}

		public Task UpdateRequest(LiquidationRequestModel liquidationRequestModel)
		{
			return liquidationRequestRepository.UpdateRequest(liquidationRequestModel);
		}
	}
}
