using MarketExpress.Application.Dto.Category.Request;
using MarketExpress.Application.Dto.Category.Response;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstacts
{
    public interface ICategoryService
    {
        Task<Result<CategoryResponse>>AddAsync(AddCategoryRequest request);
        Task<Result>SoftDeleteAsync(DeleteCategoryRequest request);
    }
}
