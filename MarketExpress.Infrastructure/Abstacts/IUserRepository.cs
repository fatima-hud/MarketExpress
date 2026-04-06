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
        Task<string>GetNationalIdFrontImageByUser(string userId);
        Task<string> GetNationalIdBackImageByUser(string userId);
    } 
    
}
