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
	public interface IPurchasingRequestService
	{
		Task<PurchasingRequestModel> CreateRequest(PurchasingRequestModel request);
		Task<List<PurchasingRequestModel>> GetPendingRequestsLv2();
		Task<PurchasingRequestModel> GetRequest(int id);
		Task<List<PurchasingRequestModel>> GetRequestsByUserId(int id);
		Task<PurchasingRequestModel> ApproveRequestLv2(ApproveRequestDto dto);
		Task<List<PurchasingRequestModel>> GetPendingRequestsLv3();
		Task<PurchasingRequestModel> ApproveRequestLv3(ApproveRequestDto dto);
		Task<PurchasingHistoryModel> CompleteRequest(CompletePurchasingRequestDto dto);
		Task<List<PurchasingRequestModel>> GetApprovedRequest();
		Task<bool> PurchasingRequestModelExists(int id);
		Task UpdateRequest(PurchasingRequestModel PurchasingRequestModel);
		Task DeleteRequest(int id);
	}
	public class PurchasingRequestService(IPurchasingRequestRepository purchasingRequestRepository) : IPurchasingRequestService
	{
		public Task<PurchasingRequestModel> ApproveRequestLv2(ApproveRequestDto dto)
		{
			return purchasingRequestRepository.ApproveRequestLv2(dto);
		}

		public Task<PurchasingRequestModel> ApproveRequestLv3(ApproveRequestDto dto)
		{
			return purchasingRequestRepository.ApproveRequestLv3(dto);
		}

		public Task<PurchasingHistoryModel> CompleteRequest(CompletePurchasingRequestDto dto)
		{
			return purchasingRequestRepository.CompleteRequest(dto);
		}

		public Task<PurchasingRequestModel> CreateRequest(PurchasingRequestModel request)
		{
			return purchasingRequestRepository.CreateRequest(request);
		}

		public Task DeleteRequest(int id)
		{
			return purchasingRequestRepository.DeleteRequest(id);
		}

		public Task<List<PurchasingRequestModel>> GetApprovedRequest()
		{
			return purchasingRequestRepository.GetApprovedRequest();
		}

		public Task<List<PurchasingRequestModel>> GetPendingRequestsLv2()
		{
			return purchasingRequestRepository.GetPendingRequestsLv2();
		}

		public Task<List<PurchasingRequestModel>> GetPendingRequestsLv3()
		{
			return purchasingRequestRepository.GetPendingRequestsLv3();
		}

		public Task<PurchasingRequestModel> GetRequest(int id)
		{
			return purchasingRequestRepository.GetRequest(id);
		}

		public Task<List<PurchasingRequestModel>> GetRequestsByUserId(int id)
		{
			return purchasingRequestRepository.GetRequestsByUserId(id);
		}

		public Task<bool> PurchasingRequestModelExists(int id)
		{
			return purchasingRequestRepository.PurchasingRequestModelExists(id);
		}

		public Task UpdateRequest(PurchasingRequestModel PurchasingRequestModel)
		{
			return purchasingRequestRepository.UpdateRequest(PurchasingRequestModel);
		}
	}
}
