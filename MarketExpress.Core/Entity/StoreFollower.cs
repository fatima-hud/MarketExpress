using MarketExpress.Core.Entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
    public class StoreFollower
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string UserId { get; set; }
        public bool IsFollow {  get; set; }
        public User User { get; set; }
        public Store Store { get; set; }
    }
}
