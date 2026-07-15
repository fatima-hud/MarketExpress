using MarketExpress.Core.Entity;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
   public interface IMessageRepository:IGenericRepositoryAsync<Message>
    {
        Task<bool>IsMessageFoundByComplaintAsync(int complaintId,int messageId);
        Task<int> MarkAsReadAsync(string userId,int complaintId);
        Task<List<Message>> GetAllMessageByComplaintIdAsync(int complaintId);
    }
}
