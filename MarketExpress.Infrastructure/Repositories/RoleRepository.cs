using MarketExpress.Core.DtoHelping.Roles;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MarketExpress.Infrastructure.Repositories
{
    public class RoleRepository : GenericRepositoryAsync<IdentityRole>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<string?> GetRoleIdByName(string roleName)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            return role?.Id;
        }
        public async Task<List<ResponseGetRolesByUserDto>> GetRolesByUserAsync(string userId)
        {
            var roles = await (
            from userRole in _dbContext.UserRoles
            join role in _dbContext.Roles
                on userRole.RoleId equals role.Id
            where userRole.UserId == userId
            select new ResponseGetRolesByUserDto
            {
                RoleId = role.Id,
                RoleName = role.Name!
            }
        ).ToListAsync();
            return roles;

        }
    }
}
