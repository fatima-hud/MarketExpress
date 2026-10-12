using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Dto.Authontication.Request;
using MarketExpress.Application.Dto.Authontication.Response;
using MarketExpress.Application.Dto.LogIn.Request;
using MarketExpress.Application.Dto.LogIn.Response;
using MarketExpress.Application.Dto.Register.Request;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Services.Auths;
using MarketExpress.Application.Services.Emails;
using MarketExpress.Application.Services.Images;
using MarketExpress.Application.Services.OtpCodes;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MarketExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IImageService _imageService;
        private readonly IOtpCodeService _otpCodeService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, UserManager<User> userManager, IEmailService emailService, IImageService imageService, IOtpCodeService otpCodeService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _userManager = userManager;
            _emailService = emailService;
            _imageService = imageService;
            _otpCodeService = otpCodeService;
            _logger = logger;
        }

        [HttpPost("Register")]
        public async Task<ApiResult> Register([FromBody] AddUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult.BadRequest(errors);

                }
                var addNewUser = await _authService.RegisterAsync(request);
                if (!addNewUser.IsSuccess)
                {
                    return this.ToApiResult(addNewUser);
                }

                return ApiResult.Created();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Erroe register");
                return ApiResult.InternalServerError("حدث خطا داخلي");
            }


        }

        [HttpPost("Login")]
        public async Task<ApiResult<LogInResponse>> Login([FromBody] LoginRequest dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<LogInResponse>.BadRequest(errors);

                }
                var log = await _authService.LoginInAsync(dto);
                if (!log.IsSuccess)
                {
                    return this.ToApiResult(log);
                }

                return this.ToApiResult(log);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error login");
                return ApiResult<LogInResponse>.InternalServerError("حدث خطا داخلي");
            }




        }


        [HttpPost("ConfirmEmail")]
        public async Task<ApiResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult.BadRequest(errors);

                }
                var result = await _otpCodeService.ConfirmEmailAsync(request);
                if (result.IsSuccess)
                    return ApiResult.Ok("تم تأكيد الايميل بنجاح");
                else
                    return this.ToApiResult(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error confirm email");
                return ApiResult.InternalServerError("حدث خطا داخلي");
            }

        }
        [HttpPost("ResendOtpCode")]
        public async Task<ApiResult> ResendOtpCode([FromBody] ResendOtpRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult.BadRequest(errors);

                }
                var result = await _otpCodeService.ResendOtpCodeAsync(request);

                if (result.IsSuccess)
                    return ApiResult.Ok("successfully!");
                else
                    return this.ToApiResult(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error resend otp code");
                return ApiResult.InternalServerError("حدث خطا داخلي");
            }

        }





        [HttpPost("RefreshToken")]
        public async Task<ApiResult<AuthResponse>> RefreshToken([FromBody] RefreshAccessTokenRequest request)


        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult<AuthResponse>.BadRequest(errors);

                }
                var result = await _authService.RefreshAccessTokenAsync(request);



                if (!result.IsSuccess)
                {
                    return this.ToApiResult(Result<AuthResponse>.Failure(result.Message, result.Errors, result.FailureType));
                }
                var response = new AuthResponse
                {
                    AccessToken = result.Value!.AccessToken,
                    RefreshToken = result.Value.RefreshToken.RefreshTokenString,
                    ExpiresIn = result.Value.RefreshToken.ExpireIn
                };
                return ApiResult<AuthResponse>.Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error refresh token");
                return ApiResult<AuthResponse>.InternalServerError("حدث خطا داخلي");
            }
        }


        [HttpPost("logout")]
        [Authorize]
        public async Task<ApiResult> Logout([FromBody] LogoutRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult.BadRequest(errors);

                }
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return ApiResult.Unauthorized();
                }
                var result = await _authService.LogoutAsync(userId, request);
                return this.ToApiResult(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error logout");
                return ApiResult.InternalServerError("حدث خطا داخلي");
            }
        }


        [HttpPost("ForgotPassword")]
        public async Task<ApiResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult.BadRequest(errors);

                }
                var result = await _authService.ForgotPasswordAsync(request);
                return this.ToApiResult(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error forgot password");
                return ApiResult.InternalServerError("حدث خطا داخلي");
            }
        }
        [HttpPost("ResetPassword")]
        public async Task<ApiResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    return ApiResult.BadRequest(errors);

                }
                var result = await _authService.ResetPasswordAsync(request);
                return this.ToApiResult(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error reset password");
                return ApiResult.InternalServerError("حدث خطا داخلي");
            }
        }
    }
}