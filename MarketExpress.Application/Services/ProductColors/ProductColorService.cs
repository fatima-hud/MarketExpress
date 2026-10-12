using MarketExpress.Application.Dto.ClothingItem.Request;
using MarketExpress.Application.Dto.ClothingItem.Response;
using MarketExpress.Application.Dto.Comment.Response;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.ProductColor.Response;
using MarketExpress.Application.Dto.ProductSize.Request;
using MarketExpress.Application.Dto.ProductSize.Response;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Application.Services.Images;
using MarketExpress.Core.DtoHelping.ClothingItem.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace MarketExpress.Application.Services.ProductColors
{
    public class ProductColorService : IProductColorService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductColorRepository _productColorRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly UserManager<User> _userManager;
        private readonly  IImageService _imageService;
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly IUserRepository _userRepository;

        public ProductColorService(IProductRepository repository, IProductColorRepository clothingItemRepository, IStoreRepository storeRepository,UserManager<User>userManager,IImageService imageService,IProductSizeRepository productSizeRepository,IUserRepository userRepository)
        {
            _productRepository = repository;
            _productColorRepository = clothingItemRepository;
            _storeRepository = storeRepository;
            _userManager = userManager;
            _imageService = imageService;
            _productSizeRepository = productSizeRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<ResponseClothingItemDto>> AddAsync(string userId, int productId, RequestAddClothingItemDto request)
        {


            var trans = await _productColorRepository.BeginTransactionAsync();
            try
            {
               
              

                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseClothingItemDto>.NotFound("المنتج غير موجود");
                }

                var store = await _storeRepository.GetByIdAsync(product.StoreId);
                if (store == null )
                {
                    await trans.RollbackAsync();
                    return Result<ResponseClothingItemDto>.NotFound("المتجر غير موجود");
                }
                var admin = store.OwnerId;
                if (admin == null  || admin != userId)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseClothingItemDto>.BadRequest("المستخدم ليس صاحب المتجر");
                }
                var isAdminActive = await _userManager.FindByIdAsync(userId);
                if (isAdminActive is not null && isAdminActive.IsDeleted)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseClothingItemDto>.BadRequest("المستخدم غير موجود");
                }

                if (string.IsNullOrWhiteSpace(request.Color))
                {
                    await trans.RollbackAsync();
                    return Result<ResponseClothingItemDto>.BadRequest("اللون مطلوب");

                }
                if (string.IsNullOrWhiteSpace(request.ColorHexCode))
                {
                    await trans.RollbackAsync();
                    return Result<ResponseClothingItemDto>.BadRequest("رمز اللون مطلوب");
                }
                if (!request.ColorHexCode.StartsWith("#"))
                {
                    await trans.RollbackAsync();
                    return Result<ResponseClothingItemDto>.BadRequest("رمز اللون غير صحيح");
                }

                var color = request.Color.ToLower().Trim();
                var isColorExist=await _productColorRepository.IsProductHasColorAsync(productId, color);
                if (isColorExist)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseClothingItemDto>.BadRequest("لون موجود مسبقا");

                }
               
                var image = await _imageService.UploadImageAsync(request.Image);
                if (image is null)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseClothingItemDto>.BadRequest("لا يمكن رفع الصورة");
                }

                var productColor = new ProductColor
                {
                    ProductId = productId,
                    Image = image.Value,
                    Color = color,
                    ColorHexCode = request.ColorHexCode

                };
                await _productColorRepository.AddAsync(productColor);
              
             

                var res = new ResponseClothingItemDto
                {
                    Id=productColor.Id,
                    productId = productColor.ProductId,
                    
                    Color = productColor.Color,
                
                    ColorHexCode = productColor.ColorHexCode,
                    Image=productColor.Image
                 



                };
                await trans.CommitAsync();
                return Result<ResponseClothingItemDto>.Success(res);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<ResponseClothingItemDto>.Failure($"خطا داخلي{ex.Message}");

            }
        }



    

        public async Task<Result<List<ResponseGetAllSizeDto>>> GetAllSizeByProductColorAsync(int productId,string color)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return Result<List<ResponseGetAllSizeDto>>.NotFound("المنتج غير موجود");
            }
            var colorL = color.ToLower().Trim();
            var productColor=await _productColorRepository.GetProductColorIdAsync(productId, colorL);
            if(productColor == null)
            {
                return Result<List<ResponseGetAllSizeDto>>.NotFound("المنتج غير موجود");
            }
            var sizes = await _productColorRepository.GetAllSizeByProductColor(productColor.Id);
            return Result<List<ResponseGetAllSizeDto>>.Success(sizes);
        }

     
        public async Task<Result<ResponseClothingItemDto>> UpdateAsync(string userId, int productColorId, RequestUpdateClothingItemDto request)
        {
            
            var productColor = await _productColorRepository.GetByIdAsync(productColorId);
            if (productColor == null)
            {
                return Result<ResponseClothingItemDto>.NotFound("العنصر غير موجود");
            }
            var product = await _productRepository.GetByIdAsync(productColor.ProductId);
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
            var isAdminActive = await _userManager.FindByIdAsync(userId);
            if (isAdminActive is not null && isAdminActive.IsDeleted)
            {
              
                return Result<ResponseClothingItemDto>.BadRequest("المستخدم غير موجود");
            }
            if (!string.IsNullOrEmpty(request.Color))
            {
                var color = request.Color.ToLower().Trim();



                var exists = await _productColorRepository.IsProductHasColorAsync(product.Id, color);


                if (exists)
                {
                    return Result<ResponseClothingItemDto>.BadRequest("المنتج متوفر مسبقا");

                }
                productColor.Color=color;


              
            }
            if (request.Image != null)
            {
                var image = await _imageService.UploadImageAsync(request.Image);
                productColor.Image = image.Value;
            }


           

            await _productColorRepository.UpdateAsync(productColor);

            var res = new ResponseClothingItemDto
            {
                productId =productColor.ProductId,
                Color = productColor.Color,
                Image = productColor.Image,
                ColorHexCode = productColor.ColorHexCode,
                Id = productColorId


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

            var res = await _productColorRepository.GetAllClothingItemByProductAsync(productId);
            var ret= new List<ResponseGetClothingItemDto>();
            
            if (res.Count==0)
            {
                return Result<List<ResponseGetClothingItemDto>>.Success(ret);
            }
            var result = res.Select(e => new ResponseGetClothingItemDto
            {
                Id = e.Id,
                Color = e.Color,
                ColorHexCode=e.ColorHexCode,
                Image = e.Image,
                ProductSizes = e.ProductSizes.Select(ps=>new ResponseGetAllSizeDto { ProductSizeId=ps.Id,Size=ps.Size,IsFoundProduct=ps.IsFoundProduct}).ToList()
            
            }).ToList();
            return Result<List<ResponseGetClothingItemDto>>.Success(result);


        }
        public async Task<Result<ResponseGetProductColorDto>> GetProductColorById(int productColorId)
        {
            var productColor = await _productColorRepository.GetByIdAsync(productColorId);
            if (productColor == null)
            {
                return Result<ResponseGetProductColorDto>.NotFound("العنصر غير موجود");
            }
            var product = await _productRepository.GetByIdAsync(productColor.ProductId);
            if (product == null)
            {
                return Result<ResponseGetProductColorDto>.NotFound("المنتج غير موجود");
            }
            var ret = new ResponseGetProductColorDto
            {
                Id = productColorId,
                Color = productColor.Color,
                Image = productColor.Image
                
                
                
                

            };
            return Result<ResponseGetProductColorDto>.Success(ret);


        }
        public async Task<Result<List<ResponseSuggestProductDto>>> GetAllSuggestClothingItemsByProductIdAsync(RequestSuggestProductDto request)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result<List<ResponseSuggestProductDto>>.NotFound("المنتج عير موجود");
            }
            var sug = await _productColorRepository.GetAllSuggestClothingItemsByProductId(request.ProductId);
            if (sug == null)
                return Result<List<ResponseSuggestProductDto>>.NotFound("لا يوجد اي اقتراحات للمنتج");
           
            return Result<List<ResponseSuggestProductDto>>.Success(sug);
        }

        public async Task<Result<List<ResponseInventoryAlertDto>>> GetAllProductInventoryAlertAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result<List<ResponseInventoryAlertDto>>.NotFound("المستخدم غير موجود");
            }
            if (user.IsBanned)
            {

                return Result<List<ResponseInventoryAlertDto>>.BadRequest("المستخدم محظور");
            }
            var store=await _storeRepository.GetStoreByAdminAsync(userId);
            if (store == null)
            {
                return Result<List<ResponseInventoryAlertDto>>.NotFound("المتجر غير موجود");
            }
            var products = await _productColorRepository.GetProductInventoryAlertAsync(store.Id);
            var ret=new List<ResponseInventoryAlertDto>();
            if(!products.Any())
            {
                return Result<List<ResponseInventoryAlertDto>>.Success(ret);
            }
            ret = products.Select(x => new ResponseInventoryAlertDto
            {
                Id = x.Id,
                ProductId = x.ProductColor.ProductId,
                Color = x.ProductColor.Color,
                ColorHexCode = x.ProductColor.ColorHexCode,
                Quantity = x.Quantity,
                Size = x.Size,
            }
                ).ToList();
            return Result<List<ResponseInventoryAlertDto>>.Success(ret);
        }

        public  async Task<Result> DeleteProductColor(string userId, int productColorId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result.NotFound("المستخدم غير موجود");
            }
            if (user.IsBanned)
            {

                return Result.BadRequest("المستخدم محظور");
            }
            var store = await _storeRepository.GetStoreByAdminAsync(userId);
            if (store == null)
            {
                return Result.NotFound("المتجر غير موجود");
            }
            var productColor = await _productColorRepository.GetProductColorWithProductAsync(productColorId);
            if (productColor == null)
            {
                return Result.NotFound("اللون غير موجود");
            }
            if(productColor.Product.StoreId != store.Id)
            {
                return Result.BadRequest("المستخدم ليس صاحب المتجر");
            }
            productColor.IsDeleted = true;
            await _productColorRepository.SaveChangesAsync();
            return Result.Success("تم الحذف بنجاح");
        }
    }
}
