using MarketExpress.Core.DtoHelping.Order;
using MarketExpress.Core.Entity;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepositoryAsync<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ResponseGetOrderByPaymentDto?> GetOrderDetailsByPaymentDto(
     int paymentId,
     int storeId)
        {
            var result = await _dbContext.Payments.IgnoreQueryFilters()
                .Where(p => p.Id == paymentId)
                .Select(p => new ResponseGetOrderByPaymentDto
                {
                    OrderId = p.OrderId,

                    UserId = p.Order.UserId,
                    UserFirstName = p.Order.User.FirstName,
                    UserLastName = p.Order.User.LastName,

                    Products = p.Order.Items
                        .Where(oi => oi.product.StoreId == storeId)
                        .Select(oi => new ResponseGetOrderByPayment
                        {
                            ProductId = oi.ProductId,
                            StoreId=oi.product.StoreId,
                            StoreName=oi.product.Store.StoreName,
                            Color = oi.Color,
                            ColorHex = oi.ColorHexCode,


                            Image = oi.product.ProductColors
                                .Where(pc => pc.Color == oi.Color)
                                .Select(pc => pc.Image)
                                .FirstOrDefault() ,

                            Quantity = oi.Quantity,
                            Price = oi.Price,
                            TotalPrice=oi.Quantity*oi.Price,
                            Size=oi.Size
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return result;
        }


        public async Task<ResponseGetOrderByPaymentDto?> GetOrderDetailsByPaymentDto( int paymentId)
        {
            var result = await _dbContext.Payments.IgnoreQueryFilters()
                .Where(p => p.Id == paymentId)
                .Select(p => new ResponseGetOrderByPaymentDto
                {
                    OrderId = p.OrderId,

                    UserId = p.Order.UserId,
                    UserFirstName = p.Order.User.FirstName,
                    UserLastName = p.Order.User.LastName,

                    Products = p.Order.Items
                       
                        .Select(oi => new ResponseGetOrderByPayment
                        {
                            ProductId = oi.ProductId,
                            StoreId=oi.product.StoreId,
                            StoreName=oi.product.Store.StoreName,
                            Color = oi.Color,
                            ColorHex = oi.ColorHexCode,

                            Image = oi.product.ProductColors
                                .Where(pc => pc.Color == oi.Color)
                                .Select(pc => pc.Image)
                                .FirstOrDefault(),

                            Quantity = oi.Quantity,
                            Price = oi.Price,
                            TotalPrice = oi.Quantity * oi.Price,
                            Size = oi.Size
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return result;
        } 
        public async Task<Payment> GetPaymentByOrderIdAsync(int orderId)
        {
            var res= await _dbContext.Payments.FirstOrDefaultAsync(x => x.OrderId == orderId);
            return res;
        }
    }
}
