using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.StoreFollower.Request;
using MarketExpress.Application.Dto.StoreFollower.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Services.StoreFollowers
{
    public class StoreFollowerService : IStoreFollowerService
    {
        private readonly IStoreFollowerRepository _storeFollowerRepository;
        private readonly UserManager<User> _userManager;
        private readonly IStoreRepository _storeRepository;
        private readonly IProductRepository _productRepository;

        public StoreFollowerService(IStoreFollowerRepository storeFollowerRepository, UserManager<User> userManager, IStoreRepository storeRepository,IProductRepository productRepository)
        {
            _storeFollowerRepository = storeFollowerRepository;
            _userManager = userManager;
            _storeRepository = storeRepository;
            _productRepository = productRepository;
        }

        public async Task<Result<List<ResponseGetProductsFollowerDto>>> GetProductsOfFollowerStoresByUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<List<ResponseGetProductsFollowerDto>>.NotFound("المستخدم غير موجود");
            }
            var productsByFollowStores=await _storeFollowerRepository.GetProductsofStoreFollowerByUserIdAsync(userId);
            var products = new List<ResponseGetProductsFollowerDto>();
            if (!productsByFollowStores.Any())
            {
                return Result<List<ResponseGetProductsFollowerDto>>.Success(products);
            }
            var ret=new List<ResponseGetProductsFollowerDto>();
            foreach (var product in productsByFollowStores)
            {
                var rating = await _productRepository.GetRatingAsync(product.Id);
                var res = new ResponseGetProductsFollowerDto
                {
                    Id = product.Id,
                    StoreId = product.StoreId,
                    Name = product.Name,
                    Image = product.Image,
                    CategoryId = product.CategoryId,
                    Description = product.Description,
                    DiscountStartDate = product.DiscountStartDate,
                    DiscountEndDate = product.DiscountEndDate,
                    DiscountPercentage = product.DiscountPercentage,
                    PriceAfterDiscount = product.PriceAfterDiscount,
                    Rating = rating,
                    Price = product.Price
                };
                ret.Add(res);
            }
            return Result<List<ResponseGetProductsFollowerDto>>.Success(ret);

        }

        public async Task<Result<ResponseGetStoreFollowerDto>> UpdateFollowAsync(string userId,RequestFollowStoreDto request)
        {
            var user=await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<ResponseGetStoreFollowerDto>.NotFound("المستخدم غير موجود");
            }
            var store=await _storeRepository.GetByIdAsync(request.StoreId);
            if (store == null)
            {
                return Result<ResponseGetStoreFollowerDto>.NotFound("المتجر غير موجود");
            }
            var storeFollower = await _storeFollowerRepository.GetStoreFollowerByUserAsync(userId, request.StoreId);
            if(storeFollower == null)
            {
                storeFollower = new StoreFollower
                {
                    UserId = userId,
                    StoreId = request.StoreId,
                    IsFollow = true

                };
                await _storeFollowerRepository.AddAsync(storeFollower);
                
            }
            else
            {
                storeFollower.IsFollow= !storeFollower.IsFollow;
                await _storeFollowerRepository.SaveChangesAsync();
            }
            var ret = new ResponseGetStoreFollowerDto
            {
                Id = storeFollower.Id,
                StoreId = storeFollower.StoreId,
                IsFollow = storeFollower.IsFollow,
            };
            return Result<ResponseGetStoreFollowerDto>.Success(ret);
            

        }
    }
}
