using MarketExpress.Core.DtoHelping.ProductInventory;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Repositories
{
    public class OrderItemRepository : GenericRepositoryAsync<OrderItem>,IOrderItemRepository
    {
        public OrderItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<OrderItem>> GetOrderItemsAsync(int orderId)
        {
            var items= await _dbContext.Orderitems.Include(e=>e.product).ThenInclude(e=>e.ProductColors).IgnoreQueryFilters().Where(e=>e.OrderId==orderId).ToListAsync();
            return items;
        }

        public async Task<List<IGrouping<int, OrderItem>>> GroupingItemsForStoresAsync(int orderId)
        {
            var res = await _dbContext.Orderitems
                 .Where(e => e.OrderId == orderId)
                 .Include(x => x.product)
                 .GroupBy(x => x.product.StoreId)
                 .ToListAsync();

            return res;
        }
        public async Task<List<ResponseGetOrderDetailesDto>> GetProductInventoryAsync(
    int storeId,
    DateTime startDate,
    DateTime endDate)
        {
           
            return await _dbContext.Orderitems.IgnoreQueryFilters()
               .Where(x =>
    x.product.StoreId == storeId &&
    x.order.CreatedAt >= startDate.Date &&
    x.order.CreatedAt < endDate.Date.AddDays(1) &&
    x.order.Status == enOrderStatus.Delivered)
                .GroupBy(x => new
                {
                    x.ProductId,
                    x.Color,
                    x.Size
                })
                .Select(g => new ResponseGetOrderDetailesDto
                {
                    ProductId = g.Key.ProductId,
                    Image = g.Select(x => x.product.Image).FirstOrDefault(),

                    Color = g.Key.Color,

                    Size = g.Key.Size,

                    ColorHexCode = g
                        .Select(x => x.ColorHexCode)
                        .FirstOrDefault(),

                    NumberOfSales = g.Sum(x => x.Quantity),

                    Price = g.Sum(x => x.Price * x.Quantity),

                    RemainingQuantity =
                        g.SelectMany(x => x.product.ProductColors)
                        .Where(c => c.Color == g.Key.Color)
                        .SelectMany(c => c.ProductSizes)
                        .Where(s => s.Size == g.Key.Size)
                        .Select(s => s.Quantity)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }




    }

    }

