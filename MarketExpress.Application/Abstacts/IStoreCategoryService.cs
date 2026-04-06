using MarketExpress.Application.Dto.Request;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstacts
{
    public interface IStoreCategoryService
    {
        Task<Result> AddAsync(string UserId,StoreCategoryDto dto);
        Task<Result>DeleteAsync(string userId,StoreCategoryDto dto );
    }
}
