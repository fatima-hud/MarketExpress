using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstacts
{
    public interface IClothingItemService
    {
        Task<Result<ResponseClothingItemDto>>AddAsync(string UserId,int productId,ClothingItemDto dto);
        Task<Result<ResponseClothingItemDto>>UpdateAsync(string UserId,int productId, int ClothItemId,ClothingItemDto dto);
        Task<Result<List<string>>> GetAllColorAsync(int productId);
        Task<Result<List<string>>> GetAllSizeAsync(int productId);
    }
}
