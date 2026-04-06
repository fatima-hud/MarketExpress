using MarketExpress.Core.Entity;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
   public interface ICartItemRepository:IGenericRepositoryAsync<CartItem>
    {
        Task<bool>CheckProductExistAsync(int cartId,int productId,string Size,string Color);
        Task<CartItem> GetCartItemAsync(int cartId, int productId);
        Task<List<CartItem>> GetCartItemsByCartId(int cartId);
        Task<decimal> GetTotalPrice(int cartId);
        
    }
}
