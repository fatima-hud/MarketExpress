using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.DeliveryLocation.Request
{
    public class RequestAddDeliveryLocationDto
    {
        public int DeliveyId { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }

    }
}
