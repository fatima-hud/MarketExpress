using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Implementations
{
    public class ClothingItemService : IClothingItemService
    {
        private readonly IProductRepository _repository;
        private readonly IClothingItemRepository _clothingItemRepository;
        private readonly IStoreRepository _storeRepository;
      
        public ClothingItemService(IProductRepository repository, IClothingItemRepository clothingItemRepository, IStoreRepository storeRepository)
        {
            _repository = repository;
            _clothingItemRepository = clothingItemRepository;
           _storeRepository = storeRepository;
        }

        public async Task<Result<ResponseClothingItemDto>> AddAsync(string userId,int productId, ClothingItemDto dto)
        {
        
       

            var product =await _repository.GetByIdAsync(productId);
            if (product == null)
            {
                return Result<ResponseClothingItemDto>.NotFound("Not found product");
            }
           
            if (product.StoreId == null)
            {
                return Result<ResponseClothingItemDto>.NotFound("not found store id");
            }
            var store= await _storeRepository.GetByIdAsync(product.StoreId);
            if (store == null)
            {
                return Result<ResponseClothingItemDto>.NotFound("Not Found store ");
            }
            var admin =  store.OwnerId;
            if(admin == null||admin!=userId)
            {
                return Result<ResponseClothingItemDto>.BadRequest("This user is not admin in this store");
            }
            var ClothingItem = new ClothingItem
            {
                ProductId = productId,
                Size = dto.Size,
                Color = dto.Color,
                IsFoundProduct = true
            };
            await _clothingItemRepository.AddAsync(ClothingItem);
            var res = new ResponseClothingItemDto
            {
                productId = productId,
                Size = dto.Size,
                Color = dto.Color,
                IsFound = true

            };
            return Result<ResponseClothingItemDto>.Success(res);
        }

       

        public async Task<Result<List<string>>> GetAllColorAsync(int productId)
        {
            var product = await _repository.GetByIdAsync(productId);
            if (product == null)
            {
                return Result<List<string>>.NotFound("Not found product");
            }
            var colors=await _clothingItemRepository.GetColorByProduct(productId);
            return Result<List<string>>.Success(colors);
        }

        public async Task<Result<List<string>>> GetAllSizeAsync(int productId)
        {
            var product = await _repository.GetByIdAsync(productId);
            if (product == null)
            {
                return Result<List<string>>.NotFound("Not found product");
            }
            var sizes = await _clothingItemRepository.GetSizeByProduct(productId);
            return Result<List<string>>.Success(sizes);
        }

        public async Task<Result<ResponseClothingItemDto>> UpdateAsync(string UserId, int productId,int ClothItemId, ClothingItemDto dto)
        {
            var product = await _repository.GetByIdAsync(productId);
            if (product == null)
            {
                return Result<ResponseClothingItemDto>.NotFound("Not found product");
            }
           
            if (product.StoreId == null)
            {
                return Result<ResponseClothingItemDto>.NotFound("not found store id");
            }
            var store = await _storeRepository.GetByIdAsync(product.StoreId);
            if (store == null)
            {
                return Result<ResponseClothingItemDto>.NotFound("Not Found store ");
            }
            var admin = store.OwnerId;
            if (admin == null || admin != UserId)
            {
                return Result<ResponseClothingItemDto>.BadRequest("This user is not admin in this store");
            }

            var clothitem = await _clothingItemRepository.GetByIdAsync(ClothItemId);
            if (clothitem == null)
            {
                return Result<ResponseClothingItemDto>.NotFound("Not found Cloth item");
            }
               if(dto.Size!=null) 
            clothitem.Size = dto.Size;
            if (dto.Color != null)
                clothitem.Color = dto.Color;
            if (dto.IsFound != null)
              clothitem.IsFoundProduct = dto.IsFound;
            await _clothingItemRepository.UpdateAsync(clothitem);

            var res = new ResponseClothingItemDto
            {
                productId = productId,
                Color = clothitem.Color,
                Size = clothitem.Size,
                IsFound = clothitem.IsFoundProduct
            };
            return Result<ResponseClothingItemDto>.Success(res);


        }
    }
}
