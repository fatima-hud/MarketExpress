using MarketExpress.Core.DtoHelping;
using MarketExpress.Core.DtoHelping.Roles;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MarketExpress.Infrastructure.Repositories
{
    public class UserRepository : GenericRepositoryAsync<User>, IUserRepository

    {
       
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
           
        }

        /*  public async Task<List<User>> GetActiveUsersAsync()
          {
              var ret = await _dbContext.Users.Where(u => !(u.IsBanned || u.IsDeleted) && u.EmailConfirmed)
                  .Select( u=> new ResponseGetActiveUserDto
                  {
                      Id = u.Id,

                      FirstName = u.FirstName,
                      LastName = u.LastName,
                      UserName = u.UserName,
                      BirthDate = u.BirthDate,
                      Email = u.Email,
                      Gender = u.Gender,
                      PhoneNumber = u.PhoneNumber,
                      ProfilePhoto = u.ProfilePhoto
                  })
                  .ToListAsync();

              return ret;
          }*/

        public async Task<List<ResponseGetActiveUserDto>> GetActiveUsersAsync()
        {
            var result = await (
                from user in _dbContext.Users
                join userRole in _dbContext.UserRoles
                    on user.Id equals userRole.UserId
                join role in _dbContext.Roles
                    on userRole.RoleId equals role.Id

                where !user.IsBanned &&
                      !user.IsDeleted &&
                      user.EmailConfirmed

                select new
                {
                    user,
                    role
                }
            ).ToListAsync();

            return result
                .GroupBy(x => x.user.Id)
                .Select(g => new ResponseGetActiveUserDto
                {
                    Id = g.Key,

                    FirstName = g.First().user.FirstName,
                    LastName = g.First().user.LastName,
                    UserName = g.First().user.UserName,
                    BirthDate = g.First().user.BirthDate,
                    Email = g.First().user.Email,
                    Gender = g.First().user.Gender,
                    PhoneNumber = g.First().user.PhoneNumber,
                    ProfilePhoto = g.First().user.ProfilePhoto,

                    Roles = g.Select(x => new ResponseGetRolesByUserDto
                    {
                        RoleId = x.role.Id,
                        RoleName = x.role.Name
                    }).ToList()
                })
                .ToList();
        }

        public async Task<List<UserRefreshToken>> GetAllRefreshTokenByUserIdAsync(string userId)
        {
            var refreshTokens = await _dbContext.UserRefreshTokens.Where(e => e.UserId == userId && !e.IsRevoked && DateTime.UtcNow <= e.ExpiresOn).ToListAsync();
            return refreshTokens;
        }

        public async Task<List<User>> GetBannedUsersAsync()
        {
            var ret = await _dbContext.Users.Where(u => u.IsBanned).ToListAsync();
            return ret;
        }


        public async Task<string> GetFullNameAsync(string userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            var fullname = user.FirstName + "  " + user.LastName;
            return fullname;

        }

        public async Task<string> GetNationalIdBackImageByUser(int storeId)
        {
            var image = await _dbContext.Documents.FirstOrDefaultAsync(e => e.StoreId == storeId && e.FileType == enFileDocumentType.NationalIdBackImage);
            return image != null ? image.FilePath : null;
        }

        public async Task<string> GetNationalIdFrontImageByUser(int storeId)
        {
            var image = await _dbContext.Documents.FirstOrDefaultAsync(e => e.StoreId == storeId && e.FileType == enFileDocumentType.NationalIdFrontImage);
            return image != null ? image.FilePath : null;
        }

        public Task<List<string>> GetRolesByStoreIdAsync(string userId, int storeId)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var res = await _dbContext.Users.FirstOrDefaultAsync(e => e.Email == email && !e.IsBanned && !e.IsDeleted);
            return res;

        }
        public async Task<User?> GetActiveUserByEmailAsync(string email)
        {
            var res = await _dbContext.Users
           .FirstOrDefaultAsync(e =>
               e.Email == email &&
               !e.IsDeleted
               );
            return res;
        }

        public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            var res = await _dbContext.Users.FirstOrDefaultAsync(e => e.PhoneNumber == phoneNumber && !e.IsBanned && !e.IsDeleted);
            return res;
        }

        public async Task<bool> PhoneNumberHasFoundAsync(string phoneNumber)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(e => e.PhoneNumber == phoneNumber && !e.IsDeleted);
            if (user != null) return true;
            return false;

        }

        public async Task<User?> GetActiveUserByUserNameAsync(string userName)
        {
            var res = await _dbContext.Users
             .FirstOrDefaultAsync(e =>
                 e.UserName == userName &&
                 !e.IsDeleted
                 );
            return res;
        }
    }
 }
