using MarketExpress.Core.DtoHelping.Order;
using MarketExpress.Core.Entity;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
   public interface IPaymentRepository:IGenericRepositoryAsync<Payment>
    {
        Task<Payment>GetPaymentByOrderIdAsync(int orderId);
        Task<ResponseGetOrderByPaymentDto> GetOrderDetailsByPaymentDto(int paymentId,int storeId);

        Task<ResponseGetOrderByPaymentDto> GetOrderDetailsByPaymentDto(int paymentId);
    }
}
