using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Message.Request
{
    public class RequestUpdateMessageDto
    {
        [Required]
        public int ComplaintId {  get; set; }
        [Required]
        public int MessageId { get; set; }
        [Required]
        public string MessageText {  get; set; }
    }
}
