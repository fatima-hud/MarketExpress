using MarketExpress.Application.Dto.Complaint.Request;
using MarketExpress.Application.Dto.Complaint.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.Complaints
{
    public interface IComplaintService
    {
        Task<Result<ResponseAddComplaintDto>> AddAsync(string userId, RequestAddComplaintDto request);
        Task<Result<List<ResponseGetComplaintDto>>> GetAllAsync(string StoreOwnerId);
    }
}
