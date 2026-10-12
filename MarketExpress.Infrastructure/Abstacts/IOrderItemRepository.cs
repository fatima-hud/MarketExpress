using MarketExpress.Core.DtoHelping.ProductInventory;
using MarketExpress.Core.Entity;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
   public interface IOrderItemRepository:IGenericRepositoryAsync<OrderItem>
    {
        Task<List<OrderItem>> GetOrderItemsAsync(int orderId);
        Task<List<IGrouping<int, OrderItem>>> GroupingItemsForStoresAsync(int orderId);
        Task<List<ResponseGetOrderDetailesDto>> GetProductInventoryAsync(int storeId, DateTime startDate, DateTime endDate);
    }
}
