using MarketExpress.Core.Entity;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
   public interface IOtpCodeRepository :IGenericRepositoryAsync<OtpCode>
    {
        Task<bool> AddCodeAsync(string userId,string code);
        Task<OtpCode> ReturnCodeAsync(string userId);
        Task<List<OtpCode>>GetAllCodesByUserId(string userId);
    }
}
