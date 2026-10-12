using MarketExpress.Application.Dto.Complaint.Request;
using MarketExpress.Application.Dto.Complaint.Response;
using MarketExpress.Core.DtoHelping.Complaint;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.Complaints
{
    public interface IComplaintService
    {
        Task<Result<ResponseAddComplaintDto>> AddAsync(string userId, RequestAddComplaintDto request);
        Task<Result<List<ResponseGetComplaintByStoreDto>>> GetAllByStoreAsync(string storeOwnerId);
        Task<Result<List<ResponseGetComplaintByUserDto>>> GetAllByUserAsync(string userId);
        
    }
}
