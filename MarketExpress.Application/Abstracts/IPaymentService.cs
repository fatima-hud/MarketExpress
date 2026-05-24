using MarketExpress.Application.Dto.Payment.Request;
using MarketExpress.Application.Dto.Payment.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstracts
{
   public interface IPaymentService
    {
       
        Task<Result<ResponseGetPaymentDto>>GetAsync(string userId,int orderId);

    }
}
