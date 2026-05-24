using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class DeliveryLocation
    {
        public int Id { get; set; }
        public int DeliveryId { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public DateTime TimeStamp { get; set; }
        public Delivery Delivery { get; set; }
    }
}
