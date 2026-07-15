using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.StoreFollower.Request;
using MarketExpress.Application.Dto.StoreFollower.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Services.StoreFollowers
{
   public interface IStoreFollowerService
    {
        Task<Result<ResponseGetStoreFollowerDto>>UpdateFollowAsync(string userId,RequestFollowStoreDto request);
        Task<Result<List<ResponseGetProductsFollowerDto>>>GetProductsOfFollowerStoresByUserAsync(string userId);
    }
}
