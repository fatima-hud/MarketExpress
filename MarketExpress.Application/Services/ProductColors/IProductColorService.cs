using MarketExpress.Application.Dto.ClothingItem.Request;
using MarketExpress.Application.Dto.ClothingItem.Response;
using MarketExpress.Application.Dto.ProductColor.Response;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.DtoHelping.ClothingItem.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.ProductColors
{
    public interface IProductColorService
    {
        Task<Result<ResponseClothingItemDto>> AddAsync(string UserId, int productId, RequestAddClothingItemDto request);
        Task<Result<ResponseClothingItemDto>> UpdateAsync(string UserId, int clothItemId, RequestUpdateClothingItemDto request);


        Task<Result<List<ResponseSuggestProductDto>>> GetAllSuggestClothingItemsByProductIdAsync(RequestSuggestProductDto request);
        Task<Result<List<ResponseGetClothingItemDto>>> GetAllClothingItemByProduct(int productId);
        Task<Result<ResponseGetProductColorDto>> GetProductColorById(int productColorId);
        Task<Result<List<ResponseGetAllSizeDto>>> GetAllSizeByProductColorAsync(int productId, string color);

        Task<Result<List<ResponseEmptyProductInventoryDto>>> GetAllEmptyProductInventoryAsync(string userId);
    }
}
