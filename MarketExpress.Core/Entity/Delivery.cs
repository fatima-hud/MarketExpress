using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class Delivery
    {
        public int Id { get; set; }
        public string DriverId { get; set; }    
        public int OrderId { get; set; }
        public DateTime StartTime{ get; set; }
        public DateTime? EndTime{ get; set; }
        public enDeliveryStatus statue { get; set; }
        public string AddressLocation { get; set; }
        public User Driver { get; set; }
        public Order Order { get; set; }
        public ICollection<DeliveryLocation> DeliveryLocations { get; set; }
    }
}
