using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
   public interface INotificationRepository:IGenericRepositoryAsync<Notification>
    {
        Task<bool> DuplicateCheckAsync(string createdById, string title, string message,enNotificationType type);


    }
}
