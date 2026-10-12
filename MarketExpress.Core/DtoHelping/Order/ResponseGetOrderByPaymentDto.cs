using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.DtoHelping.Order
{
    public class ResponseGetOrderByPaymentDto
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName {  get; set; }
        public List<ResponseGetOrderByPayment> Products { get; set; }
    }
}
