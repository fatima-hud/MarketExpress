using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.Store.Response;
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
            if (user == null || user.IsDeleted)
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
                    StoreName=product.Store.StoreName,
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

        public async Task<Result<List<ResponseStoreDto>>> GetStoreFollowByUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result<List<ResponseStoreDto>>.NotFound("المستخدم غير موجود");
            }
            var res = await _storeFollowerRepository.GetStoreFollowByUserAsync(userId);
            if(res is null)
            {
                return Result<List<ResponseStoreDto>>.Success(new List<ResponseStoreDto>());

            }
            var ret = res.Select(s => new ResponseStoreDto
            {
                Id = s.Id,
                Description = s.Description,
                Address = s.Address,
                IsActive = s.IsActive,
                FeaturedImage = s.FeaturedImage,
                StoreEmail = s.Email,
                Logo = s.Logo,
                StoreName = s.StoreName,
                StorePhoneNumber = s.PhoneNumber,
                WorkingHoursStart = s.WorkingHoursStart,
                WorkingHoursEnd = s.WorkingHoursEnd
            }).ToList();
            return Result<List<ResponseStoreDto>>.Success(ret);
            
        }

        public async Task<Result<int>> GetStoreFollowersCountAsync(int storeId)
        {
            var store = await _storeRepository.GetByIdAsync(storeId);
            if (store == null)
            {
                return Result<int>.NotFound("المتجر غير موجود");
            }
            var res = await _storeFollowerRepository.GetStoreFollowersCountAsync(storeId);
            return Result<int>.Success(res);
        }

        public async Task<Result<ResponseGetStoreFollowerDto>> UpdateFollowAsync(string userId,RequestFollowStoreDto request)
        {
            var user=await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
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
