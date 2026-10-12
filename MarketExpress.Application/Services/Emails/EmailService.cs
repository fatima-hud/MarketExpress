
using MailKit.Security;
using MarketExpress.Core.Helping;
using Microsoft.Extensions.Options;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace MarketExpress.Application.Services.Emails
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;


        public EmailService(IOptions<EmailSettings> options)

        {

            _emailSettings = options.Value;

        }


        public async Task<bool> SendEmailAsync(
        string emailAddress,
        string subject,
        string htmlMessage)
        {
            try
            {
                var email = new MimeMessage();

                email.Sender = MailboxAddress.Parse(_emailSettings.Email);
                email.From.Add(
                    new MailboxAddress("موضة", _emailSettings.Email));

                email.To.Add(MailboxAddress.Parse(emailAddress));
                email.Subject = subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = htmlMessage
                };

                email.Body = builder.ToMessageBody();
                email.Body.ContentType.Charset = "UTF-8";

                using var smtp = new SmtpClient();

                smtp.Timeout = 10000;

               

                await smtp.ConnectAsync(
                    _emailSettings.Host,
                    _emailSettings.Port,
                    SecureSocketOptions.SslOnConnect);

            

                await smtp.AuthenticateAsync(
                    _emailSettings.Email,
                    _emailSettings.Password);

               

                await smtp.SendAsync(email);
               

                await smtp.DisconnectAsync(true);

              

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("========== SMTP ERROR ==========");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("================================");

                return false;
            }
        }
    }
}
