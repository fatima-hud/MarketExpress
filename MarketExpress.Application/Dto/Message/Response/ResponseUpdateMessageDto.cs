using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Message.Response
{
    public class ResponseUpdateMessageDto
    {
        
        public int ComplaintId { get; set; }    
        public int MessageId { get; set; }
        public string MessageText { get; set; }
        public bool IsEdited { get; set; }
        public DateTime UpdatedAt {  get; set; }
        public string ReceiverId {  get; set; }
    }
}
