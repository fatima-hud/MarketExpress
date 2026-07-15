namespace MarketExpress.Application.Services.Emails
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string email, string subject, string htmlMessage);

        //  Task<bool> SendPasswordResetEmailAsync(string email, string resetCode);

    }
}
