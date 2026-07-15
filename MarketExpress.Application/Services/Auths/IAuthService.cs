using MarketExpress.Application.Dto.Authontication.Request;
using MarketExpress.Application.Dto.Authontication.Response;
using MarketExpress.Application.Dto.LogIn.Request;
using MarketExpress.Application.Dto.LogIn.Response;
using MarketExpress.Application.Dto.Register.Request;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.Auths
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(AddUserRequest request);
        Task<Result<LogInResponse>> LoginInAsync(LoginRequest dto);
        Task<Result<AuthModel>> GetTokenAsync(LoginRequest loginRequest);
        Task<Result<JwtAuthResult>> RefreshAccessTokenAsync(RefreshAccessTokenRequest request);


        Task<Result> LogoutAsync(string userId, LogoutRequest logOutDto);
        Task<Result> ForgotPasswordAsync(ForgotPasswordRequest request);
        Task<Result> ResetPasswordAsync(ResetPasswordRequest request);

      





    }
}
