using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class Complaint
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string StoreOwnerId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

    
        public User Customer { get; set; }
        public User StoreOwner { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}

