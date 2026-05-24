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
   public interface ICartItemRepository:IGenericRepositoryAsync<CartItem>
    {
        Task<CartItem> GetExistingItemAsync(int cartId,int productId,enSize Size,string Color);
        Task<CartItem> GetCartItemAsync(int cartId, int productId);
        Task<List<CartItem>> GetCartItemsByCartIdAsync(int cartId);
        Task<List<CartItem>> GetItemsByListIdsAsync(int cartId,List<int>itemsIds);
        Task<decimal> GetTotalPriceAsync(int cartId);
        Task<List<IGrouping<int, CartItem>>> GroupingItemsForStoresAsync(int cartId);

    }
}
