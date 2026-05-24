using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.ClothingItem.Request;
using MarketExpress.Application.Dto.ClothingItem.Response;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.DtoHelping.ClothingItem.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;

namespace MarketExpress.Application.Implementations
{
    public class ClothingItemService : IClothingItemService
    {
        private readonly IProductRepository _productRepository;
        private readonly IClothingItemRepository _clothingItemRepository;
        private readonly IStoreRepository _storeRepository;

        public ClothingItemService(IProductRepository repository, IClothingItemRepository clothingItemRepository, IStoreRepository storeRepository)
        {
            _productRepository = repository;
            _clothingItemRepository = clothingItemRepository;
            _storeRepository = storeRepository;
        }

        public async Task<Result<ResponseClothingItemDto>> AddAsync(string userId, int productId, RequestAddClothingItemDto request)
        {


            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return Result<ResponseClothingItemDto>.NotFound("المنتج غير موجود");
            }
            
            var store = await _storeRepository.GetByIdAsync(product.StoreId);
            if (store == null)
            {
                return Result<ResponseClothingItemDto>.NotFound("المتجر غير موجود");
            }
            var admin = store.OwnerId;
            if (admin == null || admin != userId)
            {
                return Result<ResponseClothingItemDto>.BadRequest("المستخدم ليس صاحب المتجر");
            }
            if (string.IsNullOrWhiteSpace(request.Color))
            {
                return Result<ResponseClothingItemDto>.BadRequest("اللون مطلوب");

            }  
            if (string.IsNullOrWhiteSpace(request.ColorHexCode))
            {
                return Result<ResponseClothingItemDto>.BadRequest("رمز اللون مطلوب");
            }
            if (!request.ColorHexCode.StartsWith("#"))
            {
                return Result<ResponseClothingItemDto>.BadRequest("رمز اللون غير صحيح");
            }
            if (request.Quantity <= 0)
            {
                return Result<ResponseClothingItemDto> .BadRequest("الكمية يجب ان تكون اكبر من 0");
                   
            }
        
            var color= request.Color.ToLower().Trim();
            var exists = await _clothingItemRepository.GetClothingItemByProductAsync(product.Id, request.Size , color );


            if (exists != null )
            {
                return Result<ResponseClothingItemDto>.BadRequest(" المنتج متوفر مسبقا");

            }
           
            var clothingItem = new ClothingItem
            {
                ProductId = productId,
                Size = request.Size,
                Color = color,
                Quantity = request.Quantity,
                ColorHexCode = request.ColorHexCode
               
            };
            await _clothingItemRepository.AddAsync(clothingItem);
            var res = new ResponseClothingItemDto
            {
                productId = clothingItem.ProductId,
                Size = clothingItem.Size,
                Color = clothingItem.Color,
                Quantity= clothingItem.Quantity,
                IsFound=clothingItem.IsFoundProduct,
                ColorHexCode= clothingItem.ColorHexCode
                
               

            };
            return Result<ResponseClothingItemDto>.Success(res);
        }



       
        public async Task<Result<List<ResponseGetAllColorDto>>> GetAllColorByProductSizeAsync(int productId,enSize size)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return Result<List<ResponseGetAllColorDto>>.NotFound("المنتج غير موجود");
            }

            var colors = await _clothingItemRepository.GetAllColorByProductSize(productId,size);
            return Result<List<ResponseGetAllColorDto>>.Success(colors);
        }

        public async Task<Result<List<ResponseGetAllSizeDto>>> GetAllSizeByProductColorAsync(int productId,string color)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return Result<List<ResponseGetAllSizeDto>>.NotFound("المنتج غير موجود");
            }
            var colorL = color.ToLower().Trim();

            var sizes = await _clothingItemRepository.GetAllSizeByProductColor(productId,colorL);
            return Result<List<ResponseGetAllSizeDto>>.Success(sizes);
        }

     
        public async Task<Result<ResponseClothingItemDto>> UpdateAsync(string userId, int clothItemId, RequestUpdateClothingItemDto request)
        {
            
            var clothItem = await _clothingItemRepository.GetByIdAsync(clothItemId);
            if (clothItem == null)
            {
                return Result<ResponseClothingItemDto>.NotFound("العنصر غير موجود");
            }
            var product = await _productRepository.GetByIdAsync(clothItem.ProductId);
            if (product == null)
            {
                return Result<ResponseClothingItemDto>.NotFound("المنتج غير موجود");
            }
           

            var store = await _storeRepository.GetByIdAsync(product.StoreId);
            if (store == null)
            {
                return Result<ResponseClothingItemDto>.NotFound("المتجر غير موجود");
            }
            var admin = store.OwnerId;
            if (admin == null || admin != userId)
            {
                return Result<ResponseClothingItemDto>.BadRequest("المستخدم ليس صاحب المتجر");
            }
            var color = request.Color != null? request.Color.ToLower().Trim() : clothItem.Color;
    
   

            var size = request.Size ?? clothItem.Size;
            var exists = await _clothingItemRepository.GetClothingItemByProductAsync(product.Id, size, color);
   
        
            if (exists != null && exists.Id != clothItem.Id)
            {
                return Result<ResponseClothingItemDto>.BadRequest("المنتج متوفر مسبقا");
                 
            }

            if (request.Size.HasValue)
            {
                clothItem.Size = request.Size.Value;
            }
            if (!string.IsNullOrEmpty(request.Color))
            {
                clothItem.Color = request.Color.ToLower().Trim();
            }
            if (request.Quantity.HasValue)
            {
                if (request.Quantity <= 0)
                {
                    return Result<ResponseClothingItemDto>
                        .BadRequest("الكمية يجب ان تكون اكبر من 0");
                }

                clothItem.Quantity = request.Quantity.Value;
            }

            await _clothingItemRepository.UpdateAsync(clothItem);

            var res = new ResponseClothingItemDto
            {
                productId =clothItem.ProductId,
                Color = clothItem.Color,
                Size = clothItem.Size,
                Quantity = clothItem.Quantity,
                IsFound=clothItem.IsFoundProduct
               
            };
            return Result<ResponseClothingItemDto>.Success(res);


        }


        public async Task<Result<List<ResponseGetClothingItemDto>>> GetAllClothingItemByProduct(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return Result<List<ResponseGetClothingItemDto>>.NotFound("المنتج غير موجود  ");
            }

            var res = await _clothingItemRepository.GetAllClothingItemByProductAsync(productId);
            var ret= new List<ResponseGetClothingItemDto>();
            
            if (!res.Any())
            {
                return Result<List<ResponseGetClothingItemDto>>.Success(ret);
            }
            ret = res.Select(e => new ResponseGetClothingItemDto
            {
                Id = e.Id,
                Color = e.Color,
                Quantity = e.Quantity,
                Size = e.Size,
                IsFoundProduct = e.IsFoundProduct
            }).ToList();
            return Result<List<ResponseGetClothingItemDto>>.Success(ret);


        }
        public async Task<Result<ResponseGetClothingItemDto>> GetClothingItemByProduct(int clothingItemId)
        {
            var clothingItem = await _clothingItemRepository.GetByIdAsync(clothingItemId);
            if (clothingItem == null)
            {
                return Result<ResponseGetClothingItemDto>.NotFound("العنصر غير موجود");
            }
            var product = await _productRepository.GetByIdAsync(clothingItem.ProductId);
            if (product == null)
            {
                return Result<ResponseGetClothingItemDto>.NotFound("المنتج غير موجود");
            }
            var ret = new ResponseGetClothingItemDto
            {
                Id = clothingItemId,
                Quantity = clothingItem.Quantity,
                Color = clothingItem.Color,
                Size = clothingItem.Size,
                IsFoundProduct = clothingItem.IsFoundProduct

            };
            return Result<ResponseGetClothingItemDto>.Success(ret);


        }
        public async Task<Result<List<ResponseSuggestProductDto>>> GetAllSuggestClothingItemsByProductIdAsync(RequestSuggestProductDto request)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result<List<ResponseSuggestProductDto>>.NotFound("المنتج عير موجود");
            }
            var sug = await _clothingItemRepository.GetAllSuggestClothingItemsByProductId(request.ProductId, request.Size, request.Color);
            if (sug == null)
                return Result<List<ResponseSuggestProductDto>>.NotFound("لا يوجد اي اقتراحات للمنتج");
            var res = sug.Select(e => new ResponseSuggestProductDto
            {
                ProductId = e.ProductId,
                Color = e.Color,
                Gender = e.Product.Gender,
                Occasion = e.Product.Occasion,
                Season = e.Product.Season,
                Type = e.Product.Type,
                ImageUrl = e.Product.Image,
                Name = e.Product.Name,
                Size = e.Size,
                Price = e.Product.Price,
                PriceAfterDiscount = e.Product.PriceAfterDiscount,
              //  RatingValue = e.Product.Rating
            }).ToList();
            return Result<List<ResponseSuggestProductDto>>.Success(res);
        }

       

     
    }
}
