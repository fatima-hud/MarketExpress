using MarketExpress.Core.DtoHelping.ClothingItem.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
    public interface IClothingItemRepository:IGenericRepositoryAsync<ClothingItem>
    {
        Task<bool> CheckProductIsFound(int productId, string Color, enSize Size);
        Task<List<ResponseGetAllColorDto>> GetAllColorByProduct(int productId);
      
        Task<List<ResponseGetAllSizeDto>> GetAllSizeByProduct(int productId);
       
        Task<List<ClothingItem>> GetAllSuggestClothingItemsByProductId(int productId,enSize Size,string Color);
        Task<ClothingItem> GetClothingItemByProductAsync(int productId, enSize Size, string Color);
        Task<List<ClothingItem>> GetAllClothingItemByProductAsync(int productId);
        Task<List<ResponseGetAllSizeDto>> GetAllSizeByProductColor(int productId, string color);
        Task<List<ResponseGetAllColorDto>> GetAllColorByProductSize(int productId, enSize size);
    }
}
