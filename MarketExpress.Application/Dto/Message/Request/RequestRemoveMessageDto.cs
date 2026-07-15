using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Message.Request
{
   public class RequestRemoveMessageDto
    {
     
        public int ComplaintId { get; set; }
     
        public int MessageId { get; set; }
      
    }
}
