using MarketExpress.Application.Dto.Authontication.Request;
using MarketExpress.Application.Dto.Wallet.Response;
using MarketExpress.Application.Services.Emails;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MarketExpress.Application.Services.OtpCodes
{
    public class OtpCodeService:IOtpCodeService
    {
        private readonly IOtpCodeRepository _otpCodeRepository;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        private readonly IUserRepository _userRepository;
        private readonly IWalletRepository _walletRepository;
        public OtpCodeService(IOtpCodeRepository otpCodeRepository, UserManager<User> userManager, IEmailService emailService, IUserRepository userRepository, IWalletRepository walletRepository)
        {
            _otpCodeRepository = otpCodeRepository;
            _userManager = userManager;
            _emailService = emailService;
            _userRepository = userRepository;
            _walletRepository = walletRepository;
        }

        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            if (!await CheckFormEmailAsync(request.Email))
            {
                return Result.BadRequest("@:البريد الإلكتروني يجب أن يتضمن اسم نطاق صالح ورمز");
            }
            var user = await _userRepository.GetActiveUserByEmailAsync(request.Email);

            if (user is null || user.IsDeleted)
                return Result.NotFound($"مستخدم غير موجود");
            if (user.EmailConfirmed)
            {
                return Result.BadRequest("الايميل مؤكد");
            }
            if (request.Code.Length != 6)
            {
                return Result.BadRequest(" الكود يجب ان يكون بطول 6");
            }
            if (user.ResetPasswordCode != request.Code || user.ResetPasswordCodeExpiry < DateTime.UtcNow)
            {
                return Result.BadRequest($"فشل تاكيد الايميل");
            }
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            var codes = await _otpCodeRepository.GetAllCodesByUserId(user.Id);
           
            if (codes != null)
                await _otpCodeRepository.DeleteRangeAsync(codes);
        

            var existingWallet = await _walletRepository.GetWalletByUserIdAsync(user.Id);
            if (existingWallet == null)
            {
                var wallet = new Wallet
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Balance = 0

                };
                await _walletRepository.AddAsync(wallet);


            }

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
<!DOCTYPE html>
<html lang=""ar"" dir=""rtl"">
<head>
    <meta charset=""UTF-8"">
    <title>تأكيد البريد الإلكتروني</title>
</head>

<body style=""font-family: Arial, sans-serif; background-color: #f5f5f5; padding: 30px;"">

    <div style=""max-width: 500px; margin: auto; background: white; padding: 30px; border-radius: 10px;"">

        <h2 style=""text-align: center;"">تأكيد البريد الإلكتروني</h2>

        <p>مرحبًا،</p>

        <p>
            لقد طلبت إنشاء حساب في <strong>موضة</strong>.
            استخدم رمز التحقق التالي لتأكيد بريدك الإلكتروني:
        </p>

        <div style=""text-align: center; margin: 30px 0;"">
            <span style=""
                display: inline-block;
                font-size: 32px;
                font-weight: bold;
                letter-spacing: 8px;
                padding: 15px 25px;
                background: #f0f0f0;
                border-radius: 8px;"">
                {randomNumber}
            </span>
        </div>

        <p>
            هذا الرمز مخصص لتأكيد حسابك، ولا تشاركه مع أي شخص.
        </p>

        <p>
            إذا لم تكن أنت من طلب إنشاء الحساب، يمكنك تجاهل هذه الرسالة.
        </p>

        <hr>

        <p style=""text-align: center; color: #777;"">
            فريق موضة
        </p>

    </div>

</body>
</html>";
                Console.WriteLine("Before SEND EMAIL");
                var res = await _emailService.SendEmailAsync(Email, "أكد بريدك الالكتروني", htmlMessage);
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
            try
            {
                if (!await CheckFormEmailAsync(request.Email))
                {
                    return Result.BadRequest("البريد الإلكتروني يجب أن يتضمن اسم نطاق صالح ورمز");
                }

                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                    return Result.BadRequest("ادخال خاطئ");

                var code = await CreateOtp(request.Email);

                if (code is null)
                {
                    return Result.BadRequest("فشل في ارسال تاكيد الايميل");
                }

                await using var trans =
                    await _otpCodeRepository.BeginTransactionAsync();

                try
                {
                    if (!await _otpCodeRepository.AddCodeAsync(user.Id, code))
                    {
                        await trans.RollbackAsync();
                        return Result.BadRequest("");
                    }

                    await trans.CommitAsync();

                    return Result.Success();
                }
                catch
                {
                    await trans.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                // Log ex هنا
                return Result.Failure(
                    "خطا داخلي",
                    failureType: ResultFailureType.InternalError);
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
