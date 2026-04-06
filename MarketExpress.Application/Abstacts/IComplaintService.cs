using MarketExpress.Application.Dto.Complaint.Request;
using MarketExpress.Application.Dto.Complaint.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstacts
{
    public interface IComplaintService
    {
        Task <Result<ResponseAddComplaintDto>>AddAsync(string CustomerId,RequestAddComplaintDto request);
        Task<Result<List<ResponseGetComplaintDto>>> GetAllAsync(string StoreOwnerId);
    }
}
