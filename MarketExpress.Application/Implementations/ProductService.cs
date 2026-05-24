using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Product.Request;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.Repositories;


namespace MarketExpress.Application.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageService _imageService;
        private readonly IClothingItemRepository _clothingItemRepository;

        private readonly ICategoryRepository _categoryRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IStoreCategoryRepository _storeCategoryRepository;

        public ProductService(IProductRepository productRepository, IImageService imageService, ICategoryRepository categoryRepository, IStoreRepository storeRepository, IClothingItemRepository clothingItemRepository,IStoreCategoryRepository storeCategoryRepository)
        {
            _productRepository = productRepository;
            _imageService = imageService;
            _storeRepository = storeRepository;

            _categoryRepository = categoryRepository;
            _clothingItemRepository = clothingItemRepository;
            _storeCategoryRepository = storeCategoryRepository;

        }


        public async Task<Result<ResponseProductDetailsDto>> AddAsync(string userId, RequestAddProductDto request)
        {
            var trans = await _productRepository.BeginTransactionAsync();
            try
            {
                var store = await _storeRepository.GetStoreByAdminAsync(userId);
                if (store is null)
                {
                    return Result<ResponseProductDetailsDto>.NotFound("User has not store");
                }
               
               
                var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

                if (category is null)
                {
                    return Result<ResponseProductDetailsDto>.NotFound("Not Found Category");
                }
                var isCategoryExistByStore = await _storeCategoryRepository.IsCategoryExistByStoreAsync(store.Id, category.Id);
                if (!isCategoryExistByStore)
                {
                    return Result<ResponseProductDetailsDto>.NotFound("Not Found Category in this store");
                }
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return Result<ResponseProductDetailsDto>.BadRequest("Name is required");
                }
                if (string.IsNullOrWhiteSpace(request.Description))
                {
                    return Result<ResponseProductDetailsDto>.BadRequest("Description is required");
                }
                if (request.Image is null)
                {
                    return Result<ResponseProductDetailsDto>.BadRequest("Image is required");
                }

                
                
                if (request.Price <= 0)
                {
                    return Result<ResponseProductDetailsDto>.BadRequest("Price can not be free");
                }
                var hasDiscount = request.DiscountPrecentage.HasValue || request.DiscountStartDate.HasValue ||request.DiscountEndDate.HasValue;
   
   
                if (hasDiscount)
                {
                    if (!request.DiscountPrecentage.HasValue ||
                        request.DiscountPrecentage.Value <= 0 ||
                        request.DiscountStartDate is null ||
                        request.DiscountEndDate is null)
                    {
                        return Result<ResponseProductDetailsDto>.BadRequest(
                            "Discount requires Percentage, StartDate, EndDate");
                    }

                    if (request.DiscountEndDate <= request.DiscountStartDate)
                    {
                        return Result<ResponseProductDetailsDto>.BadRequest(
                            "DiscountEndDate must be after DiscountStartDate");
                    }
                }
                var image = await _imageService.UploadImageAsync(request.Image);
                if (image is null)
                {
                    return Result<ResponseProductDetailsDto>.BadRequest("Can not upload image");
                }

                var prod = new Product
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    StoreId = store.Id ,
                    CategoryId = request.CategoryId,
                    Season = request.Season,
                    Gender = request.Gender,
                    Type = request.Type,
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
                    DiscountStartDate = prod.DiscountStartDate ,
                    DiscountEndDate = prod.DiscountEndDate ,
                    PriceAfterDiscount = prod.PriceAfterDiscount 

                };


                await trans.CommitAsync();

                return Result<ResponseProductDetailsDto>.Success(res);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<ResponseProductDetailsDto>.Failure($"Internal error {ex.Message}", failureType: ResultFailureType.InternalError);

            }


        }

        public async Task<Result> DeleteAsync(string userId, int productId)
        {

            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                return Result.NotFound("Product Not Found");

            }

            var store = await _storeRepository.GetByIdAsync(product.StoreId);
            if (store == null)
            {
                return Result.NotFound("Store Not Found");
            }
            var admin = store.OwnerId;

            if (admin == null || admin != userId)
            {
                return Result.BadRequest("This user is not admin in this store");
            }
            if (product.IsDeleted)
            {
                return Result.BadRequest("Product already deleted");
            }
            product.IsDeleted = true;
            product.DeletedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);
            return Result.Success();
        }

        public async Task<Result<List<ResponseProductDetailsDto>>> FilterAsync(FilterDto dto)
        {
            var products = await _productRepository.FilterAsync(dto.minPrice, dto.maxPrice, dto.Type.Value, dto.Color, dto.Size);
            if (products == null || products.Count == 0)
            {
                return Result<List<ResponseProductDetailsDto>>.NotFound("No products found for the given filters");
            }
            List<ResponseProductDetailsDto> res = products.Select(p => new ResponseProductDetailsDto
            {
                Id = p.Id,
                Name = p.Name,
                StoreId = p.StoreId,
                CategoryId = p.CategoryId,

                Description = p.Description,
                Price = p.Price,

             //   Rating = p.Rating,
                Image = p.Image,
               
                DiscountPercentage = p.DiscountPercentage,
                PriceAfterDiscount = p.PriceAfterDiscount

            }).ToList();
            return Result<List<ResponseProductDetailsDto>>.Success(res, "Success");

        }

        public async Task<Result<ResponseProductDetailsDto>> GetProductAsync(int productId)
        {
            var prod = await _productRepository.GetByIdAsync(productId);
            if (prod == null)
            {
                return Result<ResponseProductDetailsDto>.NotFound("Product Not Found");
            }
            var colors = await _clothingItemRepository.GetAllColorByProduct(productId);
            var sizes = await _clothingItemRepository.GetAllSizeByProduct(productId);
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

        public async Task<Result<List<ResponseProductDetailsDto>>> SearchAsync(string Query)
        {
            var products = await _productRepository.SearchAsync(Query);
            if (products == null || products.Count == 0)
            {
                return Result<List<ResponseProductDetailsDto>>.NotFound("No products found for the given filters");
            }
            List<ResponseProductDetailsDto> res = products.Select(p => new ResponseProductDetailsDto
            {
                Id = p.Id,
                Name = p.Name,

                Description = p.Description,
                StoreId = p.StoreId,
                CategoryId = p.CategoryId,
                Price = p.Price,
                DiscountPercentage = p.DiscountPercentage,
                DiscountStartDate = p.DiscountStartDate,
                DiscountEndDate = p.DiscountEndDate,
                PriceAfterDiscount = p.PriceAfterDiscount,

              //  Rating = p.Rating,
                Image = p.Image,
              



            }).ToList();



            return Result<List<ResponseProductDetailsDto>>.Success(res, "Success");


        }

        public async Task<Result<ResponseUpdateProductDto>> UpdateAsync(string userId, RequestUpdateProductDto request, int productId)
        {
            var prod = await _productRepository.GetByIdAsync(productId);
            if (prod == null)
            {
                return Result<ResponseUpdateProductDto>.NotFound("Product not found");
            }
            var store = await _storeRepository.GetByIdAsync(prod.StoreId);
            if (store == null)
            {
                return Result<ResponseUpdateProductDto>.NotFound("Store not found");
            }
            var adminId = store.OwnerId;
            if (adminId == null || adminId != userId)
            {
                return Result<ResponseUpdateProductDto>.UnAuthorized("This user is not admin in this store");
            }
            if (request.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value);
                if (category == null)
                {
                    return Result<ResponseUpdateProductDto>.NotFound("Category not found");
                }
                var isCategoryExistByStore=await _storeCategoryRepository.IsCategoryExistByStoreAsync(store.Id, category.Id);
                if (!isCategoryExistByStore)
                {
                    return Result<ResponseUpdateProductDto>.BadRequest("Category does not found in this store");
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
                    return Result<ResponseUpdateProductDto>.BadRequest("Price must be greater than 0");
                }
                prod.Price = request.Price.Value;
            }
            if (request.DiscountPrecentage.HasValue)
            {
                if (request.DiscountPrecentage.Value < 0 || request.DiscountPrecentage.Value > 100)
                {
                    return Result<ResponseUpdateProductDto>.BadRequest("Discount Precentage has to between 0 and 100");
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
                    return Result<ResponseUpdateProductDto>.BadRequest("Discount end date must be after start date");
            }
            if (prod.DiscountPercentage.HasValue &&
             (!prod.DiscountStartDate.HasValue || !prod.DiscountEndDate.HasValue))
            {
                return Result<ResponseUpdateProductDto>.BadRequest("Discount dates must be provided when setting discount percentage");
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
