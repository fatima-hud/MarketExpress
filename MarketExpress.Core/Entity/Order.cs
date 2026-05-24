using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class Order
    {
        public int Id { get; set; }
        public string UserId {  get; set; }
        public string Address { get; set; }
        public decimal TotalPrice {  get; set; }
        public DateTime CreatedAt { get; set; }
        public enOrderStatus Status { get; set; }
        public Payment Payment { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public User User { get; set; }
        public Delivery Delivery { get; set; }

        

    }
}
