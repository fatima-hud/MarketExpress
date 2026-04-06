using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.Store.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstacts
{
   public interface IStoreService
    {
    
      
        Task<Result>SoftDeleteAsync(int id);

        Task<Result<ResponseOwnerStoreDto>> GetStoreByAdminAsync(string ownerId);



        Task<Result<List<ResponseStoreDto>>> GetAllStoresAsync();



    }
}
