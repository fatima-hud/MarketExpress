using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Message.Response
{
   public class ResponseRemoveMessageDto
    {
        public int ComplaintId { get; set; }
        public int MessageId { get; set; }
        public string ReceiverId { get; set; }
    }
}
