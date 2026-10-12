using MarketExpress.Core.DtoHelping;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
    public interface IUserRepository:IGenericRepositoryAsync<User>
    {
        Task<bool>PhoneNumberHasFoundAsync(string phoneNumber);
        Task<string> GetFullNameAsync(string userId);
        Task<List<string>> GetRolesByStoreIdAsync(string userId, int storeId);
        Task<List<UserRefreshToken>>GetAllRefreshTokenByUserIdAsync(string userId);  
        Task<string>GetNationalIdFrontImageByUser(int storeId);
        Task<string> GetNationalIdBackImageByUser(int storeId);
        Task<User> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<User> GetUserByEmailAsync(string email);
        Task<List<ResponseGetActiveUserDto>> GetActiveUsersAsync();
        Task<List<User>> GetBannedUsersAsync();
        Task<User?> GetActiveUserByEmailAsync(string email);
        Task<User?> GetActiveUserByUserNameAsync(string userName);
    } 
    
}
