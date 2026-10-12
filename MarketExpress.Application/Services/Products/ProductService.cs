using MarketExpress.Application.Dto.Product.Request;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Application.Dto.Store.Response;
using MarketExpress.Application.Services.Images;
using MarketExpress.Core.DtoHelping.Admin.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;


namespace MarketExpress.Application.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageService _imageService;
        private readonly IProductColorRepository _productColorRepository;

        private readonly ICategoryRepository _categoryRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IStoreCategoryRepository _storeCategoryRepository;
        private readonly UserManager<User> _userManager;

        public ProductService(IProductRepository productRepository, IImageService imageService, ICategoryRepository categoryRepository, IStoreRepository storeRepository, IProductColorRepository clothingItemRepository, IStoreCategoryRepository storeCategoryRepository, UserManager<User> userManager)
        {
            _productRepository = productRepository;
            _imageService = imageService;
            _storeRepository = storeRepository;

            _categoryRepository = categoryRepository;
            _productColorRepository = clothingItemRepository;
            _storeCategoryRepository = storeCategoryRepository;
            _userManager = userManager;
        }


        public async Task<Result<ResponseProductDetailsDto>> AddAsync(string userId, RequestAddProductDto request)
        {
            var trans = await _productRepository.BeginTransactionAsync();
            try
            {
                var isAdminActive = await _userManager.FindByIdAsync(userId);
                if (isAdminActive is not null && isAdminActive.IsDeleted)
                {

                    return Result<ResponseProductDetailsDto>.BadRequest("المستخدم غير موجود");
                }
                var store = await _storeRepository.GetStoreByAdminAsync(userId);
                if (store is null)
                {
                    return Result<ResponseProductDetailsDto>.NotFound("مستخدم ليس لديه متجر");
                }
              


                var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

                if (category is null)
                {
                    return Result<ResponseProductDetailsDto>.NotFound("فئة غير موجودة");
                }
                var isCategoryExistByStore = await _storeCategoryRepository.IsCategoryExistByStoreAsync(store.Id, category.Id);
                if (!isCategoryExistByStore)
                {
                    return Result<ResponseProductDetailsDto>.NotFound("لا يوجد فئة في هذا المتجر");
                }
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return Result<ResponseProductDetailsDto>.BadRequest("الاسم مطلوب");
                }
                if (string.IsNullOrWhiteSpace(request.Description))
                {
                    return Result<ResponseProductDetailsDto>.BadRequest("الوصف مطلوب");
                }
                if (request.Image is null)
                {
                    return Result<ResponseProductDetailsDto>.BadRequest("الصورة مطلوبة");
                }



                if (request.Price <= 0)
                {
                    return Result<ResponseProductDetailsDto>.BadRequest("السعر لا يجب ان يكون اقل او يساوي الصفر");
                }
                var hasDiscount = request.DiscountPrecentage.HasValue || request.DiscountStartDate.HasValue || request.DiscountEndDate.HasValue;


                if (hasDiscount)
                {
                    if (!request.DiscountPrecentage.HasValue ||
                        request.DiscountPrecentage.Value <= 0 ||
                        request.DiscountStartDate is null ||
                        request.DiscountEndDate is null)
                    {
                        return Result<ResponseProductDetailsDto>.BadRequest(
                            "حقول الخصم مطلوبة");
                    }

                    if (request.DiscountEndDate <= request.DiscountStartDate)
                    {
                        return Result<ResponseProductDetailsDto>.BadRequest(
                            "تاريخ نهاية الخصم يجب ان يكون بعد تاريخ البداية");
                    }
                }
                var image = await _imageService.UploadImageAsync(request.Image);
                if (image is null)
                {
                    return Result<ResponseProductDetailsDto>.BadRequest("لا يمكن تحميل الصورة");
                }

                var prod = new Product
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    StoreId = store.Id,
                    CategoryId = request.CategoryId,
                    Season = request.Season,
                    Gender = request.Gender,
                    Type = request.Type,
                    Occasion=request.Occasion,
                    Image = image.Value,
                    DiscountPercentage = request.DiscountPrecentage,
                    DiscountStartDate = request.DiscountStartDate,
                    DiscountEndDate = request.DiscountEndDate,


                };
                await _productRepository.AddAsync(prod);



                var res = new ResponseProductDetailsDto
                {
                    Id = prod.Id,
                    Name = prod.Name,
                    Description = prod.Description,
                    StoreId = prod.StoreId,
                    CategoryId = prod.CategoryId,
                    Price = prod.Price,
                    
                    Image = prod.Image,
                    DiscountPercentage = request.DiscountPrecentage,
                    DiscountStartDate = prod.DiscountStartDate,
                    DiscountEndDate = prod.DiscountEndDate,
                    PriceAfterDiscount = prod.PriceAfterDiscount

                };


                await trans.CommitAsync();

                return Result<ResponseProductDetailsDto>.Success(res);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<ResponseProductDetailsDto>.Failure($"حدث خطا داخلي {ex.Message}", failureType: ResultFailureType.InternalError);

            }


        }

        public async Task<Result> DeleteAsync(string userId, int productId)
        {

            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                return Result.NotFound("منتج غير موجود");

            }
            var isAdminActive = await _userManager.FindByIdAsync(userId);
            if (isAdminActive is not null && isAdminActive.IsDeleted)
            {

                return Result.BadRequest("المستخدم غير موجود");
            }

            var store = await _storeRepository.GetByIdAsync(product.StoreId);
            if (store == null)
            {
                return Result.NotFound("متجر غير موجود");
            }
            var admin = store.OwnerId;

            if (admin == null || admin != userId)
            {
                return Result.BadRequest("هذا المستخدم ليس صاحب المتجر");
            }
            if (product.IsDeleted)
            {
                return Result.BadRequest("المنتج محذوف مسبقا");
            }
            product.IsDeleted = true;
            product.DeletedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);
            return Result.Success();
        }

        public async Task<Result<List<ResponseGetDiscountProductDto>>> FilterAsync(FilterDto dto)
        {
            var products = await _productRepository.FilterAsync(dto.Type, dto.Color, dto.Size,dto.minPrice,dto.maxPrice);
            

            var res = new List<ResponseGetDiscountProductDto>();
            if (products == null || products.Count == 0)

            {
                return Result<List<ResponseGetDiscountProductDto>>.Success(res);
            }
           
      

            return Result<List<ResponseGetDiscountProductDto>>.Success(products, "Success");

        }

        public async Task<Result<List<ResponseGetDiscountProductDto>>> GetAllDiscountProductByAllStores()
        {
            var res = new List<ResponseGetDiscountProductDto>();
            var products = await _productRepository.GetAllDiscountProductsByAllStoresAsync();
            if (products == null)
            {
                return Result<List<ResponseGetDiscountProductDto>>.Success(res);
            }
            res = products.Select(x => new ResponseGetDiscountProductDto
            {
                Id = x.Id,
                StoreId = x.StoreId,
                Name = x.Name,
                Price = x.Price,
                PriceAfterDiscount = x.PriceAfterDiscount,
                DiscountPercentage = x.DiscountPercentage,
                DiscountStartDate = x.DiscountStartDate,
                DiscountEndDate = x.DiscountEndDate,
                Image = x.Image
            }).ToList();
            return Result<List<ResponseGetDiscountProductDto>>.Success(res);
        }

        public async Task<Result<List<ResponseGetDiscountProductByStoreDto>>> GetAllDiscountProductByStore(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result<List<ResponseGetDiscountProductByStoreDto>>.NotFound("المستخدم غير موجود");
            }
            if (user.IsBanned)
            {

                return Result<List<ResponseGetDiscountProductByStoreDto>>.BadRequest("المستخدم محظور");
            }
            var store = await _storeRepository.GetStoreByAdminAsync(userId);
            if (store == null)
            {
                return Result<List<ResponseGetDiscountProductByStoreDto>>.NotFound("المتجر غير موجود");
            }
            var res = new List<ResponseGetDiscountProductByStoreDto>();
            var products = await _productRepository.GetAllDiscountProductsByStoreAsync(store.Id);
            if (products == null)
            {
                return Result<List<ResponseGetDiscountProductByStoreDto>>.Success(res);
            }
            res = products.Select(x => new ResponseGetDiscountProductByStoreDto
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                PriceAfterDiscount = x.PriceAfterDiscount,
                DiscountPercentage = x.DiscountPercentage,
                DiscountStartDate = x.DiscountStartDate,
                DiscountEndDate = x.DiscountEndDate,
                Image = x.Image
            }).ToList();
            return Result<List<ResponseGetDiscountProductByStoreDto>>.Success(res);
        }

        public async Task<Result<ResponseProductDetailsDto>> GetProductAsync(int productId)
        {
            var prod = await _productRepository.GetByIdAsync(productId);
            if (prod == null)
            {
                return Result<ResponseProductDetailsDto>.NotFound("منتج غير موجود");
            }
            var colors = await _productColorRepository.GetAllColorByProduct(productId);
            var sizes = await _productColorRepository.GetAllSizeByProduct(productId);
            var rating = await _productRepository.GetRatingAsync(productId);
            var res = new ResponseProductDetailsDto
            {
                Id = prod.Id,
                Description = prod.Description,
                StoreId = prod.StoreId,
                CategoryId = prod.CategoryId,
                Price = prod.Price,
                Rating = rating,
                Image = prod.Image,

                Name = prod.Name,
                DiscountPercentage = prod.DiscountPercentage,
                DiscountStartDate = prod.DiscountStartDate,
                DiscountEndDate = prod.DiscountEndDate,
                PriceAfterDiscount = prod.PriceAfterDiscount,
                Colors = colors,
                Sizes = sizes
            };
            return Result<ResponseProductDetailsDto>.Success(res);

        }

        public async Task<Result<ResponseProductDashboardDto>> GetProductDashboardsAsync(string userId, int pageNumber,int pageSize)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result<ResponseProductDashboardDto>.NotFound("مستخدم غير موجود");
            }
           
            var store = await _storeRepository.GetStoreByAdminAsync(userId);
            if (store is null)
            {
                
                return Result<ResponseProductDashboardDto>.NotFound("متجر غير موجود");
            }
            if(pageNumber <=0)
            {
                pageNumber = 1;
            }
            if (pageSize <= 0)
                pageSize = 10;

            if (pageSize > 50)
                pageSize = 50;

            var ret = await _productRepository.GetProductDashboardsAsync(store.Id,pageNumber,pageSize);
             return Result<ResponseProductDashboardDto>.Success(ret);
            


        }

        public async Task<Result<List<ResponseGetDiscountProductDto>>> SearchAsync(string Query)
        {
            var products = await _productRepository.SearchAsync(Query);
            var res = new List<ResponseGetDiscountProductDto>();
            if (products == null || products.Count == 0)

            {
                return Result<List<ResponseGetDiscountProductDto>>.Success(res);
            }
            res = products.Select(p => new ResponseGetDiscountProductDto
            {
                Id = p.Id,
                Name = p.Name,

                StoreId = p.StoreId,

                Price = p.Price,
                DiscountPercentage = p.DiscountPercentage,
                DiscountStartDate = p.DiscountStartDate,
                DiscountEndDate = p.DiscountEndDate,
                

                PriceAfterDiscount = p.PriceAfterDiscount,

                Image = p.ProductColors
    .FirstOrDefault(c =>
        Query.ToLower().Contains(c.Color.ToLower()))
    ?.Image
    ?? p.Image,

            }).ToList();



            return Result<List<ResponseGetDiscountProductDto>>.Success(res, "Success");


        }

        public async Task<Result<ResponseUpdateProductDto>> UpdateAsync(string userId, RequestUpdateProductDto request, int productId)
        {
            var prod = await _productRepository.GetByIdAsync(productId);
            if (prod == null)
            {
                return Result<ResponseUpdateProductDto>.NotFound("منتج غير موجود");
            }
            var store = await _storeRepository.GetByIdAsync(prod.StoreId);
            if (store == null)
            {
                return Result<ResponseUpdateProductDto>.NotFound("متجر غير موجود");
            }
            var adminId = store.OwnerId;
            if (adminId == null || adminId != userId)
            {
                return Result<ResponseUpdateProductDto>.UnAuthorized("هذاالمستخدم ليس صاحب المتجر");
            }
            if (request.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value);
                if (category == null)
                {
                    return Result<ResponseUpdateProductDto>.NotFound("فئة غير موجودة");
                }
                var isCategoryExistByStore = await _storeCategoryRepository.IsCategoryExistByStoreAsync(store.Id, category.Id);
                if (!isCategoryExistByStore)
                {
                    return Result<ResponseUpdateProductDto>.BadRequest("الفئة غير موجودة في هذا المتجر");
                }
                prod.CategoryId = category.Id;

            }
            if (request.Image != null)
            {
                var img = await _imageService.UploadImageAsync(request.Image);
                prod.Image = img.Value;
            }
            


            if (request.Price.HasValue)
            {
                if (request.Price.Value <= 0)
                {
                    return Result<ResponseUpdateProductDto>.BadRequest("السعر يجب ان يكون اكبر من 0");
                }
                prod.Price = request.Price.Value;
            }
            if (request.DiscountPrecentage.HasValue)
            {
                if (request.DiscountPrecentage.Value < 0 || request.DiscountPrecentage.Value > 100)
                {
                    return Result<ResponseUpdateProductDto>.BadRequest("نسبة الخصم يجب ان تكون بين ال 0و100");
                }
                prod.DiscountPercentage = request.DiscountPrecentage.Value;
            }
            if (request.DiscountStartDate.HasValue)
            {
                prod.DiscountStartDate = request.DiscountStartDate.Value;
            }
            if (request.DiscountEndDate.HasValue)
            {
                prod.DiscountEndDate = request.DiscountEndDate.Value;
            }
            if (prod.DiscountStartDate.HasValue && prod.DiscountEndDate.HasValue)
            {
                if (prod.DiscountStartDate > prod.DiscountEndDate)
                    return Result<ResponseUpdateProductDto>.BadRequest("تاريخ نهاية الخصم يجب ان تكون اكبر من تاريخ البداية");
            }
            if (prod.DiscountPercentage.HasValue &&
             (!prod.DiscountStartDate.HasValue || !prod.DiscountEndDate.HasValue))
            {
                return Result<ResponseUpdateProductDto>.BadRequest("يجب تضمين تاريخ بداية ونهاية الخصم");
            }
            if (!string.IsNullOrEmpty(request.Name))
            {
                prod.Name = request.Name;
            }
            if (!string.IsNullOrEmpty(request.Description))
            {
                prod.Description= request.Description;
            }




            await _productRepository.UpdateAsync(prod);

            var res = new ResponseUpdateProductDto
            {
                Id = prod.Id,
                Name = prod.Name,
                Description = prod.Description,
                StoreId = prod.StoreId,
                CategoryId = prod.CategoryId,

                Price = prod.Price,

                Image = prod.Image,
                DiscountPercentage = prod.DiscountPercentage,
                DiscountStartDate = prod.DiscountStartDate,
                DiscountEndDate = prod.DiscountEndDate,
                PriceAfterDiscount = prod.PriceAfterDiscount
                



            };
            return Result<ResponseUpdateProductDto>.Success(res);

        }
    }
}
