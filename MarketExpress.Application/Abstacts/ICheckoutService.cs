using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstacts
{
    public interface ICheckoutService
    {
        Task<Result<CheckoutDto>> CheckoutAsync(string userId,string Address,int StoreId);
    }
}
