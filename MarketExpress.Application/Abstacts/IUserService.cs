
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstacts
{
   public interface IUserService
    {
     
        Task<bool> CheckFormPhoneNumberAsync(string phoneNumber);
        Task<Result<string>> UpdateProfilePhoto(string userId,IFormFile file);
        
    }
}
