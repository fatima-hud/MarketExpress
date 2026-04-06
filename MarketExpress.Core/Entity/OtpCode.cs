using MarketExpress.Core.Entity.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class OtpCode
    {
        public int Id{ get; set; }
        public string Code {  get; set; }
        public string UserId { get; set; }
        public DateTime Expiry {  get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }
      

    }
}
