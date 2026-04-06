using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Response
{
    public class RefreshTokenResult
    {
        public string UserName { get; set; }
        public string RefreshTokenString { get; set; }
        public DateTime ExpireIn { get; set; }
    }
}
