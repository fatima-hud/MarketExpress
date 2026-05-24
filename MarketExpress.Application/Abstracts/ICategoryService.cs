using MarketExpress.Application.Dto.Category.Request;
using MarketExpress.Application.Dto.Category.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Abstracts
{
    public interface ICategoryService
    {
        Task<Result<ResponseCategory>> AddAsync(RequestAddCategory request);
        Task<Result> SoftDeleteAsync(int categoryId);
        Task<Result<List<ResponseCategory>>>GetAllCategoryAsync();
    }
}
