using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.DtoHelping.Order
{
    public class ResponseGetOrderByPayment
    {
        public int ProductId { get; set; }
        public int StoreId {  get; set; }
        public string StoreName { get; set; }   
        public int Quantity { get; set; }
        public enSize Size { get; set; }
        public string Color { get; set; }
        public string ColorHex { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public decimal TotalPrice {  get; set; }
    }
}
