using MarketExpress.Core.DtoHelping.Roles;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Infrastructure.Abstacts
{
    public interface IRoleRepository : IGenericRepositoryAsync<IdentityRole>
    {
        Task<string?> GetRoleIdByName(string roleName);
        Task<List<ResponseGetRolesByUserDto>> GetRolesByUserAsync(string userId);
    }
}
