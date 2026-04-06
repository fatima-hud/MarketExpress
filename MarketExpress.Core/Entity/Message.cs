using MarketExpress.Core.Entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
    public class Message
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public string SenderId { get; set; }
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }



        public Complaint Complaint { get; set; }
        public User Sender { get; set; }
    }
}
