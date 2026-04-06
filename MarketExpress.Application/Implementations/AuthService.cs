using MarketExpress.Application.Abstracts;
using MarketExpress.Core.Results;
using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Core.Results;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Cryptography;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using MarketExpress.Application.Dto.Register.Request;
using Org.BouncyCastle.Asn1.Pkcs;
using MarketExpress.Core.Helping;
using MarketExpress.Application.Dto.LogIn.Request;
using MarketExpress.Application.Dto.Authontication.Response;
using MarketExpress.Application.Dto.LogIn.Response;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.Entity.Identity;
using Microsoft.Extensions.Options;
using MarketExpress.Application.Dto;
using Microsoft.VisualBasic.FileIO;
using MarketExpress.Infrastructure.Abstracts;
using MarketExpress.Application.Dto.Authontication.Request;

using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
namespace MarketExpress.Application.Implementations
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        private readonly IOtpCodeRepository _otpCodeRepository;
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole>  _roleManager;
        private readonly SignInManager<User> _signInManager;
     
        private JwtSettings _jwtSettings { get; }
        private IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(UserManager<User> userManager, IUserRepository userRepository, IEmailService emailService, IOtpCodeRepository otpCodeRepository, IUserService userService, RoleManager<IdentityRole> roleManager, IOptions<JwtSettings> jwtSettings, SignInManager<User> signInManager, IRefreshTokenRepository refreshTokenRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _emailService = emailService;
            _otpCodeRepository = otpCodeRepository;
            _userService = userService;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _signInManager = signInManager;
            _refreshTokenRepository = refreshTokenRepository;
        }




        public async Task<Result<LogInResponse>> LoginInAsync(LoginRequest dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                return Result<LogInResponse>.UnAuthorized("Email or Password not found");
            }
            if (user.IsBanned)
            {
                return Result<LogInResponse>.Failure("User is banned", failureType: ResultFailureType.Forbidden);
            }
            if (!user.EmailConfirmed)
            {
                return Result<LogInResponse>.BadRequest("Email Not Confirm");
            }
            var pass = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!pass.Succeeded)
            {

                return Result<LogInResponse>.UnAuthorized("Email or Password not found");
            }

            await RemoveOldRefreshTokens(user.Id);

            var roles = await _userManager.GetRolesAsync(user);
            var LGetRolesDto = new List<GetRolesDto>();
            foreach (var role in roles)
            {
                var x = new GetRolesDto
                {
                    Name = role
                };
                LGetRolesDto.Add(x);

            }
            var jwt = await GetJWTAndRerfreshToken(user);
            var res = new LogInResponse
            {
                JwtAuthResult = jwt,
                GetRoles = LGetRolesDto
            };



            return Result<LogInResponse>.Success(res);






        }

        public async Task<JwtAuthResult> GetJWTAndRerfreshToken(User user)
        {
            var accessToken = await GenerateJwtToken(user);
            var token = new JwtSecurityTokenHandler().WriteToken(accessToken);
            var refreshToken = GetRefreshToken(user.UserName!);

            var userRefreshToken = new UserRefreshToken
            {
                CreatesOn = DateTime.Now,
                ExpiresOn = refreshToken.ExpireIn,
              
                IsRevoked = false,
                RefreshToken = refreshToken.RefreshTokenString,
                Token = token,
                UserId = user.Id
            };
            await _refreshTokenRepository.AddAsync(userRefreshToken);

            var response = new JwtAuthResult()
            {
                RefreshToken = refreshToken,
                AccessToken = token
            };

            return response;
        }

        private async Task RemoveOldRefreshTokens(string userId)
        {
            var oldTokens = await _refreshTokenRepository.GetByUserIdAsync(userId);
            foreach (var token in oldTokens)
            {
                await _refreshTokenRepository.DeleteAsync(token);
            }
        }
        public async Task<JwtSecurityToken> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var jwtToken = new JwtSecurityToken(
               _jwtSettings.Issuer,
               _jwtSettings.Audience,
               claims,
               expires: DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpireMinute),
               signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.SecretKey)), SecurityAlgorithms.HmacSha256Signature)
               );






            return jwtToken;

        }


        private RefreshTokenResult GetRefreshToken(string username)
        {
            var refreshToken = new RefreshTokenResult
            {
                ExpireIn = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDay),
                UserName = username,
                RefreshTokenString = GenerateRefreshToken().Token
            };
            return refreshToken;
        }
        private UserRefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            var randomNumberGenerate = RandomNumberGenerator.Create();
            randomNumberGenerate.GetBytes(randomNumber);

            // Console.WriteLine($"Refresh Token Expire Date: {_jwtSettings.RefreshTokenExpireDate}");

            return new UserRefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),


                ExpiresOn = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpireDay),
                CreatesOn = DateTime.UtcNow,

            };

        }

        public async Task<Result> RegisterAsync(AddUserRequest request)
        {

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                BirthDate = request.BirthDate,
                Gender = request.Gender
            };
            var trans = await _userRepository.BeginTransactionAsync();

            try
            {

                var existUserName = await _userManager.FindByNameAsync(user.UserName);
                if (existUserName != null)
                {
                    
                    return Result.BadRequest("UserName is Exist");
                    
                }
                var existUser = await _userManager.FindByEmailAsync(user.Email);
                if (existUser != null)
                {
                  
                    return Result.BadRequest("Email is Exist");
                }

                var FormEmail = await _emailService.CheckFormEmailAsync(user.Email);
                if (!FormEmail)
                {
                  
                    return Result.BadRequest("The email should contain a valid domain and '@' symbol.");
                }




                var exitPhoneNumber = await _userRepository.PhoneNumberHasFoundAsync(request.PhoneNumber);
                if (exitPhoneNumber)
                {
                   
                    return Result.BadRequest("PhoneNumber is Exist");
                }
                if (!await _userService.CheckFormPhoneNumberAsync(request.PhoneNumber))
                {
                  
                    return Result.BadRequest("PhoneNumber is Invalid");
                }
                if (request.BirthDate < BirthDateSettings.minDate || request.BirthDate > BirthDateSettings.maxDate)
                {
                   
                    return Result.BadRequest("Birth date is not valid");
                }

                if (!Enum.IsDefined(typeof(enGender), request.Gender))
                {
                  
                    return Result.BadRequest("Gender must be 0 (Male) or 1 (Female)");
                }
            var createResult = await _userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                

                return Result.BadRequest(string.Join(",",
                createResult.Errors.Select(x => x.Description).ToList()));
            }

          
            

                await _userManager.AddToRoleAsync(user, "User");
                //Send Confirm Email
                var code = await _emailService.CreateOtp(user.Email);
               
                if (code is null)
                {
                   
                   

                    await trans.RollbackAsync(); 
                    
                   
                    return Result.BadRequest("Faild Send Email Confrimation");
                }
                if (!await _otpCodeRepository.AddCodeAsync(user.Id, code))
                {
                   

                    await trans.RollbackAsync();
                    return Result.BadRequest("something was wrong");
                }



                await trans.CommitAsync();
                return Result.Success();
            }

            catch (Exception ex)
            {
                await trans.RollbackAsync();


                var error = ex.InnerException?.Message ?? ex.Message;
                throw new Exception(error);
            

        }


        }



        public async Task<Result<AuthModel>> GetTokenAsync(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
            {
                return Result<AuthModel>.NotFound("Email or Password not found");
            }
            var pass = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

            if (pass == false)
            {

                return Result<AuthModel>.UnAuthorized("User is not authorized");
            }
            var jwtsecuritytoken = await GenerateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            var authmodel = new AuthModel
            {
                Email = loginRequest.Email,
                Username = user.UserName,
                Roles = roles.ToList(),
                //ExpiresOn = jwtsecuritytoken.ValidTo,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtsecuritytoken)
            };
            if (user.RefreshTokens.Any(e => e.IsActive()))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(e => e.IsActive());
                authmodel.RefreshToken = activeRefreshToken.Token;
                authmodel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;

            }
            else
            {
                var refreshtoken = GenerateRefreshToken();
                authmodel.RefreshToken = refreshtoken.Token;
                authmodel.RefreshTokenExpiration = refreshtoken.ExpiresOn;
                user.RefreshTokens.Add(refreshtoken);
                await _userManager.UpdateAsync(user);

            }
            return Result<AuthModel>.Success(authmodel);
        }


       public async Task<Result> LogoutAsync(string userId,LogoutRequest logOutDto)
        {
            var token = await _refreshTokenRepository.GetByRefreshTokenAsync(logOutDto.RefreshToken);
           
            if (token is null)
            {
                return Result.Failure("Refresh token not found.");
            }

            if (token.UserId != userId)
            {
                return Result.UnAuthorized("RefreshToken does not belong to the current user.");
            }
            if (token.IsExpired)
            {
                return Result.BadRequest("Refresh token has expired");
            }
            if (token.IsRevoked)
            {
                return Result.BadRequest("Refresh token was already revoked");
            }

        
                token.IsRevoked = true;
                token.RevokeOn = DateTime.UtcNow;
                await _refreshTokenRepository.UpdateAsync(token);
            

            return Result.Success("Logout Success");
        }
       

      

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = _jwtSettings.validateIssuer,
                ValidateAudience = _jwtSettings.validateAudience,
                ValidateIssuerSigningKey = _jwtSettings.validateIssuerSigningKey,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                ValidateLifetime = false // تجاهل انتهاء الصلاحية
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
               !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");
                return principal;
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            /* var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            ;*/

        }

        public async Task<Result<JwtAuthResult>> RefreshAccessTokenAsync(RefreshAccessTokenRequest request)
        {


            try
            {
                var principal = GetPrincipalFromExpiredToken(request.AccessToken);
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(request.AccessToken);
                //var userId = principal.FindFirstValue(nameof(UserClaimModel.Id));
                var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

                var user = await _userManager.FindByIdAsync(userId!);
                if (user == null)
                {
                    return Result<JwtAuthResult>.NotFound("User Not Found");
                }

                var validationResult = await ValidateRefreshTokenAsync(request.AccessToken, request.RefreshToken, user.Id);
                if (!validationResult.IsSuccess)
                    return Result<JwtAuthResult>.Failure(validationResult.Message, validationResult.Errors);

                var oldRefreshToken = await _refreshTokenRepository.GetByRefreshTokenAsync(request.RefreshToken);
                // حذف  Refresh Token في قاعدة البيانات
               

                await _refreshTokenRepository.DeleteAsync(oldRefreshToken!);



                var result = await GetJWTAndRerfreshToken(user);
                return Result<JwtAuthResult>.Success(result);

            }
            catch (SecurityTokenException ex)
            {
              //  Console.WriteLine("SecurityTokenException: " + ex.Message);
                return Result<JwtAuthResult>.BadRequest("Invalid AccessToken: " + ex.Message);
            }
            catch (Exception ex)
            {
               // Console.WriteLine("Exception: " + ex.Message);
                return Result<JwtAuthResult>.BadRequest("Invalid AccessToken: " + ex.Message);
            }

            /* catch (Exception)
             {
                 return Result<JwtAuthResult>.BadRequest("Invalid AccessToken ");
                 throw new SecurityTokenException();
             }*/


        }
        private async Task<Result> ValidateRefreshTokenAsync(string accessToken, string refreshToken, string userId)
        {
            var storedToken = await _refreshTokenRepository.GetByRefreshTokenAsync(refreshToken);
                                                           

            if (storedToken == null)
                return Result.Failure("Invalid refresh token");

            if (storedToken.Token != accessToken)
                return Result.Failure("Token mismatch");

            if (storedToken.UserId != userId)
                return Result.Failure("User mismatch");

            if (storedToken.ExpiresOn < DateTime.UtcNow)
                return Result.Failure("Refresh token expired");

            if (storedToken.IsRevoked)
                return Result.Failure("Refresh token revoked");

            return Result.Success();
        }

        public async Task<Result> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result.NotFound("User not found");

            //Generate Random Number

            var code = await _emailService.CreateOtp(request.Email);
            if (code is null)
            {
               

                return Result.BadRequest("Faild Send Email Confrimation");
            }
            if (!await _otpCodeRepository.AddCodeAsync(user.Id, code))
            {
               
                return Result.BadRequest("something was wrong");
            }

           
          

            return Result.Success();
        }
        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
        {
            using var trans = await _userRepository.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    return Result.NotFound("User not found");
                if (user.ResetPasswordCode != request.Code ||
                    user.ResetPasswordCodeExpiry < DateTime.UtcNow)
                {
                    await trans.RollbackAsync();
                    return Result.Failure("Invalid or expired reset code");
                }
                var RemovePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!RemovePasswordResult.Succeeded)
                {
                    await trans.RollbackAsync();
                    return Result.BadRequest(string.Join(", ", RemovePasswordResult.Errors.Select(e => e.Description)));
                }
                if (await _userManager.HasPasswordAsync(user))
                {
                    await trans.RollbackAsync();
                    return Result.BadRequest("Failed Remove Password");
                }
                var AddNewPasswordresult = await _userManager.AddPasswordAsync(user, request.NewPassword);
                if (!AddNewPasswordresult.Succeeded)
                {
                    await trans.RollbackAsync();
                    return Result.BadRequest(string.Join(", ", AddNewPasswordresult.Errors.Select(e => e.Description)));
                }
                var codes = await _otpCodeRepository.GetAllCodesByUserId(user.Id);
                if(codes != null) 
                await _otpCodeRepository.DeleteRangeAsync(codes);

                await trans.CommitAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result.Failure("internal error", failureType: ResultFailureType.InternalError);
            }
        }
    }
    
}
