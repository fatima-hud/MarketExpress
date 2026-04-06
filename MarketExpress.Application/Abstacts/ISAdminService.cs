using MarketExpress.Application.Dto;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstacts
{
    public interface ISAdminService
    {
        Task<Result<bool>> RevokeTokenAsync(string userId); 
        Task<Result> AddRoleAsync(string SadminId, AddRoleDto dto);
    }
}
