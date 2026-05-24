using MarketExpress.Application.Dto.Authontication.Request;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Abstracts
{
    public interface IEmailService 
    {
        Task<bool> SendEmailAsync(string email, string subject, string htmlMessage);
     
      //  Task<bool> SendPasswordResetEmailAsync(string email, string resetCode);

    }
}
