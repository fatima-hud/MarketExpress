using MarketExpress.Application.Dto.Authontication.Request;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Abstracts
{
    public interface IOtpCodeService
    {
        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
        Task<string?> CreateOtp(string Email);
        Task<Result> ResendOtpCodeAsync(ResendOtpRequest request);
        Task<bool> CheckFormEmailAsync(string email);
    }
}
