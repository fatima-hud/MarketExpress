
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
                Console.WriteLine("SMTP CONNECT START");
                using (var smtp = new SmtpClient())
                {
                    smtp.Timeout = 10000;
                    // Connect to the SMTP server using the settings
                    await smtp.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.SslOnConnect);
                    //  await smtp.ConnectAsync(_emailSettings.Host,_emailSettings.Port,SecureSocketOptions.StartTls );

                    // Authenticate using the email credentials
                    await smtp.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
                    // تعيين وقت الانتظار بالمللي ثانية (10 ثوانٍ)

                    // Send the email
                    await smtp.SendAsync(email);

                    // Disconnect after sending the email
                    await smtp.DisconnectAsync(true);
                }
                Console.WriteLine("SMTP CONNECT End");
                return true;
            }
            catch (Exception ex)
            {
            

                return false;
            }
        }

    }

}

