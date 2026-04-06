using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.RequestStore.Request
{
   public class RequestRejectdDto
    {
        public int RequestId { get; set; }
       
        public string RejectionReason { get; set; }
    }
}
