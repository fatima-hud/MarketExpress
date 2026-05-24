using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Authontication.Request;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarketExpress.Application.Implementations
{
    public class OtpCodeService:IOtpCodeService
    {
        private readonly IOtpCodeRepository _otpCodeRepository;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public OtpCodeService(IOtpCodeRepository otpCodeRepository, UserManager<User> userManager, IEmailService emailService)
        {
            _otpCodeRepository = otpCodeRepository;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            if (!await CheckFormEmailAsync(request.Email))
            {
                return Result.BadRequest("The email should contain a valid domain and '@' symbol");
            }
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return Result.NotFound($"User Not Found");
            if (user.EmailConfirmed)
            {
                return Result.BadRequest("Email was confirmed");
            }
            if (request.Code.Length != 6)
            {
                return Result.BadRequest("The verification code must be 6 digits ");
            }
            if (user.ResetPasswordCode != request.Code || user.ResetPasswordCodeExpiry < DateTime.UtcNow)
            {
                return Result.BadRequest($"Invalid confirmation code");
            }
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            var codes = await _otpCodeRepository.GetAllCodesByUserId(user.Id);
            if (codes != null)
                await _otpCodeRepository.DeleteRangeAsync(codes);

            return Result.Success();
        }
        public async Task<string?> CreateOtp(string Email)
        {
            try
            {

                var chars = "0123456789";
                var random = new Random();
                var randomNumber = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());


                var htmlMessage = $@"
<h3>Confirm your email</h3>
<p>Please enter this code to confirm your account:</p>
<P> Code : '{randomNumber}'</P>
<p>If you did not create this account, ignore this email.</p>";

                Console.WriteLine("Before SEND EMAIL");
                var res = await _emailService.SendEmailAsync(Email, "Confirm your email", htmlMessage);
                Console.WriteLine("AFTER SEND EMAIL");
                if (!res) return null;
                return randomNumber;
            }
            catch (Exception ex)
            {
                return null;
            }



        }
        public async Task<Result> ResendOtpCodeAsync(ResendOtpRequest request)
        {
            var trans = await _otpCodeRepository.BeginTransactionAsync();
            try
            {
                if (!await CheckFormEmailAsync(request.Email))
                {
                    return Result.BadRequest("The email should contain a valid domain and '@' symbol");
                }
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    return Result.BadRequest("invalid user");



                var code = await CreateOtp(request.Email);

                if (code is null)
                {
                    await trans.RollbackAsync();
                    return Result.BadRequest("Failed to send email confirmation");
                }
                if (!await _otpCodeRepository.AddCodeAsync(user.Id, code))
                {
                    await trans.RollbackAsync();
                    return Result.BadRequest("");
                }


                await trans.CommitAsync();
                return Result.Success();
            }

            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result.Failure("Internal error", failureType: ResultFailureType.InternalError);

            }
        }

        public async Task<bool> CheckFormEmailAsync(string email)
        {

            string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";

            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }


    }
}
