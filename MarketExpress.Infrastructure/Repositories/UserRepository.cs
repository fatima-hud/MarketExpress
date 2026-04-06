using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;

namespace MarketExpress.Infrastructure.Repositories
{
    public class UserRepository :GenericRepositoryAsync<User>, IUserRepository

    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<UserRefreshToken>> GetAllRefreshTokenByUserIdAsync(string userId)
        {
            var refreshTokens=await _dbContext.UserRefreshTokens.Where(e=>e.UserId==userId && !e.IsRevoked && DateTime.UtcNow <= e.ExpiresOn).ToListAsync();
            return refreshTokens;
        }

        public async Task<string> GetFullNameAsync(string userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            var fullname = user.FirstName +"  " + user.LastName; 
            return fullname;

        }

        public async Task<string> GetNationalIdBackImageByUser(string userId)
        {
            var image = await _dbContext.Documents.FirstOrDefaultAsync(e => e.UserId == userId && e.Type == enDocumentType.NationalIdBackImage);
            return image != null ? image.FilePath : null;
        }

        public async Task<string> GetNationalIdFrontImageByUser(string userId)
        {
            var image=await _dbContext.Documents.FirstOrDefaultAsync(e=>e.UserId == userId && e.Type==enDocumentType.NationalIdFrontImage );
            return image != null ? image.FilePath : null;
        }

        public  Task<List<string>> GetRolesByStoreIdAsync(string userId, int storeId)
        {
            throw new NotImplementedException();
        }

        /* public async Task<List<string>> GetRolesByStoreIdAsync(string userId, int storeId)
         {
           var res= await _dbContext.UserRoles.Where(e=>e.UserId == userId&&e.).ToListAsync()  
         }*/

        public async Task<bool> PhoneNumberHasFoundAsync(string phoneNumber)
        {
            var user= await _dbContext.Users.FirstOrDefaultAsync(e=>e.PhoneNumber==phoneNumber);
            if (user != null) return true;
            return false;

        }
    }
}
