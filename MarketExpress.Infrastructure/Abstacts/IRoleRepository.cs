using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
    public interface IRoleRepository :IGenericRepositoryAsync<IdentityRole>
    {
        Task<string>GetRoleIdByName(string roleName);
    }
}
