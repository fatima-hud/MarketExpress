using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Authontication.Response
{
   public class AuthResponse
    {
       
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
            public DateTime ExpiresIn { get; set; }
        
    }
}
