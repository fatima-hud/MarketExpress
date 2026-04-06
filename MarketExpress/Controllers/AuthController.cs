using MarketExpress.Core.Results;
using MarketExpress.Api.Bases;
using MarketExpress.Api.Extention;
using MarketExpress.Application.Abstacts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Identity;
using MarketExpress.Application.Abstracts;
using Microsoft.AspNetCore.Http.HttpResults;
using MarketExpress.Application.Dto.Register.Request;
using MarketExpress.Application.Dto.Authontication.Request;
using MarketExpress.Application.Dto.LogIn.Request;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.LogIn.Response;
using MarketExpress.Application.Dto.Authontication.Response;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Application.Implementations;
using MarketExpress.Application.Dto;
using Microsoft.AspNetCore.Authorization;
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
    

        public AuthController(IAuthService authService, UserManager<User> userManager ,IEmailService emailService,IImageService imageService)//IUserService userService )
        {
            _authService = authService;
            _userManager = userManager;
            _emailService = emailService;
            _imageService = imageService;
     
        }

        [HttpPost("Register")]
        public async Task<ApiResult> Register([FromBody]AddUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult.BadRequest(ModelState.ToString());
            }
            var addNewUser = await _authService.RegisterAsync(request);
            if (!addNewUser.IsSuccess)
            {
                return this.ToApiResult(addNewUser);
            }
            
            return ApiResult.Created();


        }

        [HttpPost("Login")]
        public async Task<ApiResult<LogInResponse>> Login([FromBody]LoginRequest dto)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult<LogInResponse>.BadRequest(ModelState.ToString());
            }
            var log = await _authService.LoginInAsync(dto);
            if (!log.IsSuccess)
            {
                return this.ToApiResult(log);
            }
            
            return this.ToApiResult(log);




        }
       

        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody]ConfirmEmailRequest request)
        {

           
            var result = await _emailService.ConfirmEmailAsync(request);
            if (result.IsSuccess)
                return ApiResult.Ok("Email confirmed successfully!");
            else
                return this.ToApiResult(result);
                
        }
        [HttpPost("ResendOtpCode")]
        public async Task<IActionResult> ResendOtpCode([FromBody]ResendOtpRequest request)
        {
            var result=await _emailService.ResendOtpCodeAsync(request);
           
            if (result.IsSuccess)
                return ApiResult.Ok("successfully!");
            else
                return this.ToApiResult(result);

        }


     


        [HttpPost("RefreshToken")]
        public async Task<ApiResult<AuthResponse>> RefreshToken([FromBody] RefreshAccessTokenRequest request)
                
            {
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
     

        [HttpPost("logout")]
        [Authorize]
        public async Task<ApiResult> Logout([FromBody] LogoutRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ApiResult.BadRequest("Invalid logout request.");
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId) )
            {
                return ApiResult.Unauthorized();
            }
            var result = await _authService.LogoutAsync(userId,request);
            return this.ToApiResult(result);
        }

      
        [HttpPost("ForgotPassword")]
        public async Task<ApiResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _authService.ForgotPasswordAsync(request);
            return this.ToApiResult(result);
        }
        [HttpPost("ResetPassword")]
        public async Task<ApiResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);
            return this.ToApiResult(result);
        }
    }
}