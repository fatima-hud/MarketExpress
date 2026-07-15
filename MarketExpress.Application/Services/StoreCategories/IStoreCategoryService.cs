using MarketExpress.Application.Dto.StoreCategory.Request;
using MarketExpress.Application.Dto.StoreCategory.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.StoreCategories
{
    public interface IStoreCategoryService
    {
        Task<Result<ResponseGetStoreCategoryDto>> AddAsync(string userId, RequestAddStoreCategoryDto dto);
        Task<Result> DeleteAsync(string userId, int storeCategoryId);
        Task<Result<List<ResponseGetStoreCategoryDto>>> GetAllStoreCategoryAsync(string userId);
        Task<Result<List<ResponseGetStoreCategoryDto>>> GetAllStoreCategoryAsync(int storeId);

    }
}
