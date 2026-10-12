using MarketExpress.Core.DtoHelping.Complaint;
using MarketExpress.Core.Entity;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
   public interface IComplaintRepository:IGenericRepositoryAsync<Complaint>
    {
        Task<List<Complaint>> GetAllComplaintByAdminAsync(string adminId);
        Task<Complaint>GetComplaintAsync(string userId,string storeOwnerId);
        Task<List<ResponseGetComplaintByUserDto>> GetAllComplaintByUserAsync(string userId);
    }
}
