
using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstracts
{
   public interface IUserService
    {
     
        Task<bool> CheckFormPhoneNumberAsync(string phoneNumber);
        Task<Result<string>> UpdateProfilePhoto(string userId,IFormFile file);
        Task<Result> CancelRequestAsync(string userId, int storeId);
        Task<Result<ResponseUpdateRequestStoreDto>> UpdateInfoRequestAsync(string userId, int storeId, RequestUpdateRequestStoreDto request);
       


    }
}
