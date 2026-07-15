using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Message.Response
{
    public class ResponseMessageDto
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string ?SenderImageUrl { get; set; }
        public string ReceiverId { get; set; }
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }
}
