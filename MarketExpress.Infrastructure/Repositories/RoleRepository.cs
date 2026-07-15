using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
