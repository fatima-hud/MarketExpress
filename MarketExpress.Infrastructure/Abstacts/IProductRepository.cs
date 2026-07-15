using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
   public interface IProductRepository:IGenericRepositoryAsync<Product>
    {
        Task<List<Product>> SearchAsync(string Query);
        Task<List<Product>> FilterAsync(enType? Type,string?Color,enSize ?Size);

       Task<bool> IsProductHasSizeAsync(int productId, enSize size);
        Task<bool> IsProductHasColorAsync(int productId, string Color);
        Task<double> GetRatingAsync(int productId);
        Task<Store> GetStoreByProductIdAsync(int productId);
        Task<List<Product>>GetAllProductByStoreAsync(int storeId);
        Task<List<Product>> GetAllDiscountProductsByStoreAsync(int storeId);
        Task<List<Product>> GetAllDiscountProductsByAllStoresAsync();
      




    }
}
