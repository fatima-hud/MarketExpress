using MarketExpress.Application.Dto.Authontication.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.LogIn.Response
{
   public class LogInResponse
    {
        public List<GetRolesDto> GetRoles { get; set; } 
        public JwtAuthResult JwtAuthResult { get; set; }
    }
}
