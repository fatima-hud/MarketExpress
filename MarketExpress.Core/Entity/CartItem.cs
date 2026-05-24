using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId {  get; set; }

        public int ProductId {  get; set; }
        public int Quantity {  get; set; }
        public enSize Size {  get; set; }
        public string Color {  get; set; }
        public string ColorHexCode { get; set; }
        public Cart Cart { get; set; }
        public Product Product { get; set; }
        
    }
}
