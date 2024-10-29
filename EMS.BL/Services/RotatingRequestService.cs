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
    public interface IRotatingRequestService
    {
        Task<RotatingRequestModel> CreateRequest(RotatingRequestModel request);
        Task<List<RotatingRequestModel>> GetPendingRequestsLv2();
        Task<RotatingRequestModel> GetRequest(int id);
        Task<RotatingRequestModel> ApproveRequestLv2(ApproveRequestDto dto);
        Task<List<RotatingRequestModel>> GetPendingRequestsLv3();
        Task<RotatingRequestModel> ApproveRequestLv3(ApproveRequestDto dto);
        Task<RotatingHistoryModel> CompleteRequest(CompleteRequestDto dto);
        Task<List<RotatingRequestModel>> GetApprovedRequest();

    }
    public class RotatingRequestService(IRotatingRequestRepository rotatingRequestRepository) : IRotatingRequestService
    {
        public Task<RotatingRequestModel> ApproveRequestLv2(ApproveRequestDto dto)
        {
            return rotatingRequestRepository.ApproveRequestLv2(dto);
        }

        public Task<RotatingRequestModel> ApproveRequestLv3(ApproveRequestDto dto)
        {
            return rotatingRequestRepository.ApproveRequestLv3(dto);
        }

        public Task<RotatingHistoryModel> CompleteRequest(CompleteRequestDto dto)
        {
            return rotatingRequestRepository.CompleteRequest(dto);
        }

        public Task<RotatingRequestModel> CreateRequest(RotatingRequestModel request)
        {
            return rotatingRequestRepository.CreateRequest(request);
        }

        public Task<RotatingRequestModel> GetRequest(int id)
        {
            return rotatingRequestRepository.GetRequest(id);
        }

        public Task<List<RotatingRequestModel>> GetPendingRequestsLv2()
        {
            return rotatingRequestRepository.GetPendingRequestsLv2();
        }

        public Task<List<RotatingRequestModel>> GetPendingRequestsLv3()
        {
            return rotatingRequestRepository.GetPendingRequestsLv3();
        }

        public Task<List<RotatingRequestModel>> GetApprovedRequest()
        {
            return rotatingRequestRepository.GetApprovedRequest();
        }
    }
}
