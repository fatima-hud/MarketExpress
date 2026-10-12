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
  public  interface IOrderRepository:IGenericRepositoryAsync<Order>
        
    {
        Task<List<Order>> GetAllOrderAsync(string userId);
        Task<List<Order>> GetAllFilterOrdersAsync(enOrderStatus orderStatus, int pageNumber, int pageSize);
    }
}
