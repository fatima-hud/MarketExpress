using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class ClothingItem
    {
        public int Id { get; set; }
        public int ProductId {  get; set; }
        public string Color {  get; set; }
        public string ColorHexCode { get; set; }
        public enSize Size{  get; set; }
        public int Quantity { get; set; }

        public bool IsFoundProduct => Quantity > 0;
        public Product Product { get; set; }


    }
}
