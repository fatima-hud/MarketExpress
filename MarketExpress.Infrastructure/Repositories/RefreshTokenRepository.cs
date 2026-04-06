
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstracts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace MarketExpress.Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepositoryAsync<UserRefreshToken>, IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public RefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
        public async Task<UserRefreshToken?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _dbcontext.UserRefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.RefreshToken == refreshToken);
        }

        public async Task<UserRefreshToken?> GetByTokenAsync(string token)
        {
            return await _dbcontext.UserRefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task<List<UserRefreshToken>> GetByUserIdAsync(string userId)
        {
            return await _dbcontext.UserRefreshTokens
                .Where(e=>e.UserId==userId)
                .ToListAsync();
        }

        public async Task<bool> IsRefreshTokenValidAsync(string refreshToken, string userId)
        {
            var token = await GetByRefreshTokenAsync(refreshToken);
            return token != null &&
                   token.IsActive() &&
                   token.RevokeOn==null;
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var token = await GetByRefreshTokenAsync(refreshToken);
            if (token != null)
            {
                token.RevokeOn=DateTime.UtcNow;
                await UpdateAsync(token);
            }
        }

        public async Task RevokeAllRefreshTokensAsync(string userId)
        {
            var tokens = await GetByUserIdAsync(userId);
            foreach (var token in tokens.Where(t => DateTime.UtcNow < t.ExpiresOn && !t.IsRevoked))
            {
                token.RevokeOn = DateTime.UtcNow;
                token.IsRevoked = true;
            }
            await UpdateRangeAsync(tokens);
        }

        public async Task CleanExpiredRefreshTokensAsync()
        {
            var expiredTokens = await _dbcontext.UserRefreshTokens
                .Where(rt => rt.IsExpired || rt.IsRevoked)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                _dbcontext.UserRefreshTokens.RemoveRange(expiredTokens);
                await _dbcontext.SaveChangesAsync();
            }
        }

    }
}

