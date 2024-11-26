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
    public interface IMaintenanceRequestService
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

	public class MaintenanceRequestService(IMaintenanceRequestRepository maintenanceRequestRepository) : IMaintenanceRequestService
	{
		public Task<MaintenanceRequestModel> ApproveRequestLv2(ApproveRequestDto dto)
		{
			return maintenanceRequestRepository.ApproveRequestLv2(dto);
		}

		public Task<MaintenanceRequestModel> ApproveRequestLv3(ApproveRequestDto dto)
		{
			return maintenanceRequestRepository.ApproveRequestLv3(dto);
		}

		public Task<MaintenanceRequestModel> CompleteApproval(int id)
		{
			return maintenanceRequestRepository.CompleteApproval(id);
		}

		public Task<MaintenanceHistoryModel> CompleteMaintenance(CompletePurchasingRequestDto dto)
		{
			return maintenanceRequestRepository.CompleteMaintenance(dto);
		}

		public Task<MaintenanceRequestModel> CreateRequest(MaintenanceRequestModel request)
		{
			return maintenanceRequestRepository.CreateRequest(request);
		}

		public Task DeleteRequest(int id)
		{
			return maintenanceRequestRepository.DeleteRequest(id);
		}

		public Task<List<MaintenanceRequestModel>> GetApprovedRequest()
		{
			return maintenanceRequestRepository.GetApprovedRequest();
		}

		public Task<List<MaintenanceRequestModel>> GetPendingRequestsLv2()
		{
			return maintenanceRequestRepository.GetPendingRequestsLv2();
		}

		public Task<List<MaintenanceRequestModel>> GetPendingRequestsLv3()
		{
			return maintenanceRequestRepository.GetPendingRequestsLv3();
		}

		public Task<MaintenanceRequestModel> GetRequest(int id)
		{
			return maintenanceRequestRepository.GetRequest(id);
		}

		public Task<List<MaintenanceRequestModel>> GetRequestsByUserId(int id)
		{
			return maintenanceRequestRepository.GetRequestsByUserId(id);
		}

		public Task<bool> MaintenanceRequestModelExists(int id)
		{
			return maintenanceRequestRepository.MaintenanceRequestModelExists(id);
		}

		public Task UpdateRequest(MaintenanceRequestModel maintenanceRequest)
		{
			return maintenanceRequestRepository.UpdateRequest(maintenanceRequest);
		}
	}
}
