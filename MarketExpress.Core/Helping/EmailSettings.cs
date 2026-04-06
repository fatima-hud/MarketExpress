using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Helping
{
    public class EmailSettings
    {
        public int Port { get; set; }
        public string Host { get; set; }
        public string Email { get; set; }
        
        public string Password { get; set; }
    }
}
