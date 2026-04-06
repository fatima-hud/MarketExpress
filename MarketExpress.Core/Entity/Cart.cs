using MarketExpress.Core.Entity.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId {  get; set; }
    
        public IEnumerable<CartItem> CartItems { get; set; }
        public User User { get; set; }
     

       
    }
}
