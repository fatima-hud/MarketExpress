  using MarketExpress.Core.Entity.Identity;

using MarketExpress.Infrastructure.InfrastructureBases;

namespace MarketExpress.Infrastructure.Abstracts
{
    public interface IRefreshTokenRepository : IGenericRepositoryAsync<UserRefreshToken>
    {
        Task<UserRefreshToken?> GetByRefreshTokenAsync(string refreshToken);
        Task<UserRefreshToken?> GetByTokenAsync(string token);
        Task<List<UserRefreshToken>> GetByUserIdAsync(string userId);
        Task<bool> IsRefreshTokenValidAsync(string refreshToken, string userId);
        Task RevokeRefreshTokenAsync(string refreshToken);
        Task RevokeAllRefreshTokensAsync(string userId);
        Task CleanExpiredRefreshTokensAsync();

    }
}
  