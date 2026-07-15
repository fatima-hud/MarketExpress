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
    public interface IProductColorRepository:IGenericRepositoryAsync<ProductColor>
    {
        Task<bool> CheckProductIsFound(int productId, string Color, enSize Size);
        Task<List<ResponseGetAllColorDto>> GetAllColorByProduct(int productId);
      
        Task<List<ResponseGetAllSizeDto>> GetAllSizeByProduct(int productId);
       
        Task<List<ResponseSuggestProductDto>> GetAllSuggestClothingItemsByProductId(int productId);
        Task<bool> IsProductColorHasSizeAsync(int productId, enSize Size, string Color);
        Task<List<ProductColor>> GetAllClothingItemByProductAsync(int productId);
        Task<List<ResponseGetAllSizeDto>> GetAllSizeByProductColor(int productColorId);
       
        Task<List<ProductSize>> GetEmptyProductInventoryAsync(int storeId);
        Task<string>GetClothingItemImageAsync(int productId,string color);
        Task<ProductColor> GetProductColorIdAsync(int productId, string color);
        Task<int> GetProductByProductColorAsync(int productColorId);
        Task<bool>IsProductHasColorAsync(int productId,string color);
    }
}
