using MarketExpress.Core.Entity;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
   public interface IDeliveryLocationRepository: IGenericRepositoryAsync<DeliveryLocation>
    {
        Task<List<DeliveryLocation>> GetAllLocationByDeliveryAsync(int deliveryId);

    }
}
