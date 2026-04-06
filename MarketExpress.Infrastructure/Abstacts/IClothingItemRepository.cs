using MarketExpress.Core.Entity;
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
        Task<bool> CheckProductIsFound(int productId, string Color, string Size);
        Task<List<string>> GetColorByProduct(int productId);
        Task<List<string>> GetSizeByProduct(int productId);
    }
}
