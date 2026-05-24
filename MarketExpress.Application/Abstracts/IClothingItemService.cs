using MarketExpress.Application.Dto.ClothingItem.Request;
using MarketExpress.Application.Dto.ClothingItem.Response;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.DtoHelping.ClothingItem.Response;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstracts
{
    public interface IClothingItemService
    {
        Task<Result<ResponseClothingItemDto>>AddAsync(string UserId,int productId,RequestAddClothingItemDto request);
        Task<Result<ResponseClothingItemDto>>UpdateAsync(string UserId, int clothItemId,RequestUpdateClothingItemDto request);
      
        Task<Result<List<ResponseSuggestProductDto>>> GetAllSuggestClothingItemsByProductIdAsync(RequestSuggestProductDto request);
        Task<Result<List<ResponseGetClothingItemDto>>> GetAllClothingItemByProduct(int productId);
        Task<Result<ResponseGetClothingItemDto>> GetClothingItemByProduct(int clothingItemId);
        Task<Result<List<ResponseGetAllSizeDto>>> GetAllSizeByProductColorAsync(int productId, string color);
        Task<Result<List<ResponseGetAllColorDto>>> GetAllColorByProductSizeAsync(int productId, enSize size);
    }
}
