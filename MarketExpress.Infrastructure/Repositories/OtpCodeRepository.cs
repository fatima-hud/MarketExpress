using MarketExpress.Core.Entity;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Repositories
{
    public class OtpCodeRepository : GenericRepositoryAsync<OtpCode>,IOtpCodeRepository
    {
        public OtpCodeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddCodeAsync(string userId, string code)
        {
            var otp = new OtpCode
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Code = code,
         
            };
            var res = await _dbContext.Set<OtpCode>().AddAsync(otp);
            await _dbContext.SaveChangesAsync();
            otp.Expiry = otp.CreatedAt.AddMinutes(5);
            var user=await _dbContext.Users.FindAsync(userId);
            user.ResetPasswordCode = code;
            user.ResetPasswordCodeExpiry = otp.Expiry;


            
            await _dbContext.SaveChangesAsync();
            if (res != null) return true;
            return false;
        }

        public  async Task<List<OtpCode>> GetAllCodesByUserId(string userId)
        { 
            var res=await _dbContext.OtpCodes.Where(e=>e.UserId == userId).ToListAsync();
            return res;
        }

        public async Task<OtpCode> ReturnCodeAsync(string userId)

        {
           var code= await _dbContext.Set<OtpCode>().Where(e => e.UserId == userId).OrderByDescending(e=>e.CreatedAt).FirstOrDefaultAsync();
            if (code != null) return code;
            return null;

              
        }
    }
}
