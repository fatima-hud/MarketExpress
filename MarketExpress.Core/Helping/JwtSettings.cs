using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Helping
{
   public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public bool validateIssuer { get; set; }
        public bool validateAudience { get; set; }
        public bool validateLifetime { get; set; }
        public bool validateIssuerSigningKey { get; set; }
        public int AccessTokenExpireMinute { get; set; }
        public int RefreshTokenExpireDay { get; set; }

    }
}
