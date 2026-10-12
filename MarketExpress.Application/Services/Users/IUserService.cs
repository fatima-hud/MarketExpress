using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Application.Dto.User;
using MarketExpress.Core.DtoHelping;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Http;

namespace MarketExpress.Application.Services.Users
{
    public interface IUserService
    {

        Task<bool> CheckFormPhoneNumberAsync(string phoneNumber);
        Task<Result<string>> UpdateProfilePhoto(string userId, IFormFile file);
        Task<Result> CancelRequestAsync(string userId, int storeId);
        Task<Result<ResponseUpdateRequestStoreDto>> UpdateInfoRequestAsync(string userId, int storeId, RequestUpdateRequestStoreDto request);
        Task<Result<ResponseUserDto>>GetUserAsync(string userId);
        Task<Result<List<ResponseGetActiveUserDto>>>GetActiveUsersAsync();
        Task<Result<List<ResponseUserDto>>> GetBannedUsersAsync();
        Task<Result>DeleteUserAsync(string userId);
        Task<Result<ResponseUserDto>> UpdateUserAsync(string userId, RequestUpdateUserDto request);






    }
}
