
using MarketExpress.Application.Abstracts;
using MailKit.Net.Smtp;
using MimeKit;
using MarketExpress.Core.Results;
using System.Net.Mail;
using System.Net;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Microsoft.Extensions.Logging;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MarketExpress.Core.Helping;
using Microsoft.Extensions.Options;
using MailKit.Security;
using MarketExpress.Infrastructure.Abstacts;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MarketExpress.Database;
using System.Text.RegularExpressions;
using MarketExpress.Application.Dto.Authontication.Request;
using MarketExpress.Core.Entity.Identity;
using Azure.Core;
using MarketExpress.Core.Entity;


namespace MarketExpress.Application.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IOtpCodeRepository _otpCodeRepository;
        private readonly ApplicationDbContext _context;
        private readonly  UserManager<User> _userManager;

        public EmailService(IOptions<EmailSettings> options,IOtpCodeRepository otpCodeRepository,ApplicationDbContext context,UserManager<User> userManager)

        {
            _emailSettings = options.Value;
            _otpCodeRepository = otpCodeRepository;
            _context = context;
            _userManager = userManager;
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

       

        public async Task<bool> SendEmailAsync(string Email, string subject, string htmlMessage)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_emailSettings.Email);
                email.From.Add(new MailboxAddress("MarketExpress App", _emailSettings.Email));
                email.To.Add(MailboxAddress.Parse(Email));
                email.Subject = subject;


                var builder = new BodyBuilder
                {
                    HtmlBody = htmlMessage // Set HTML content
                };
                email.Body = builder.ToMessageBody();
                
                email.Body.ContentType.Charset = "UTF-8";

                // Create an SmtpClient using MailKit and connect to SMTP server

                using (var smtp = new SmtpClient())
                {
                    // Connect to the SMTP server using the settings
                    await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.SslOnConnect);
                    //  await smtp.ConnectAsync(_emailSettings.Host,_emailSettings.Port,SecureSocketOptions.StartTls );

                    // Authenticate using the email credentials
                    await smtp.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
                    smtp.Timeout = 10000; // تعيين وقت الانتظار بالمللي ثانية (10 ثوانٍ)

                    // Send the email
                    await smtp.SendAsync(email);

                    // Disconnect after sending the email
                    await smtp.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
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
                var res = await SendEmailAsync(Email, "Confirm your email", htmlMessage);
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
            var trans = await _context.Database.BeginTransactionAsync();
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

