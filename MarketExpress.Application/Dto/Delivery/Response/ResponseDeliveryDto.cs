using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Delivery.Response
{
    public class ResponseDeliveryDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string DriverId{ get; set; }
        public string DriverName{ get; set; }
        public string DriverPhoneNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string AddressLocation{ get; set; }
        public enDeliveryStatus Status { get; set; }  
    }
}
