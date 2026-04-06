using MarketExpress.Application.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Authontication.Response
{
    public class JwtAuthResult
    {
        public string AccessToken { get; set; }
        public RefreshTokenResult RefreshToken { get; set; }
    }
}
