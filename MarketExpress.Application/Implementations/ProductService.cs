using Azure.Core;
using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;

using MarketExpress.Core.Entity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1.Ocsp;
using Request = Org.BouncyCastle.Asn1.Ocsp.Request;
using static System.Net.Mime.MediaTypeNames;
using Org.BouncyCastle.Bcpg.Attr;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using MarketExpress.Application.Dto.Product.Request;
using MarketExpress.Application.Dto.Product.Response;


namespace MarketExpress.Application.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageService _imageService;
       
     
        private readonly ICategoryRepository _categoryRepository;
        private readonly IStoreRepository _storeRepository;

        public ProductService(IProductRepository productRepository, IImageService imageService, ICategoryRepository categoryRepository,IStoreRepository storeRepository)
        {
            _productRepository = productRepository;
            _imageService = imageService;
         _storeRepository= storeRepository;
           
            _categoryRepository = categoryRepository;
            
        }

      
        public async Task<Result<ProductResponse>> AddAsync(string userId, ProductRequest request)
        {
            var trans=await _productRepository.BeginTransactionAsync();
            try
            {
                var store = await _storeRepository.GetByIdAsync(request.StoreId);
                if(store is null)
                {
                    return Result<ProductResponse>.NotFound("Not Found Store");
                }

                var adminId = store.OwnerId;
                if (adminId == null || adminId != userId)
                {
                    return Result<ProductResponse>.BadRequest("This user is not admin in this store");
                }
                var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

                if (category is null)
                {
                    return Result<ProductResponse>.NotFound("Not Found Category");
                }
               
                var image = await _imageService.UploadImageAsync(request.Image);
                if(image is null)
                {
                    return Result<ProductResponse>.BadRequest("Can not upload image");
                }
               
                var prod = new Product
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    StoreId=request.StoreId,
                    CategoryId=request.CategoryId,
                    Quantity = request.Quantity,
                    Season = request.Season,
                    Gender = request.Gender,
                    Type = request.Type,
                    Image = image.Value,
                    DiscountPercentage=request.DiscountPrecentage,
                    DiscountStartDate=request.DiscountStartDate,
                    DiscountEndDate=request.DiscountEndDate,


                };
                await _productRepository.AddAsync(prod);
              //  prod.PriceAfterDiscount = prod.DiscountPercentage != null ? prod.Price - (prod.Price * prod.DiscountPercentage / 100) : null;                

                  



                await _productRepository.UpdateAsync(prod);

                var res = new ProductResponse
                {
                    Id = prod.Id,
                    Name = prod.Name,
                    Description = prod.Description,
                    StoreId=prod.StoreId,
                    CategoryId=prod.CategoryId,
                    Price = prod.Price,
                    Quantity = prod.Quantity,
                    Image = prod.Image,
                    DiscountPercentage= request.DiscountPrecentage,
                    DiscountStartDate = prod.DiscountStartDate,
                    DiscountEndDate = prod.DiscountEndDate,
                    PriceAfterDiscount = prod.PriceAfterDiscount,

                };


                await trans.CommitAsync();

                return Result<ProductResponse>.Success(res);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<ProductResponse>.Failure("Internal error", failureType: ResultFailureType.InternalError);

            }


        }

        public async Task<Result> DeleteAsync(string userId, int Id)
        {
           
            var product=await _productRepository.GetByIdAsync(Id);

            if (product == null)
            {
                return Result.NotFound("Product Not Found");

            }
            var store = await _storeRepository.GetByIdAsync(product.StoreId);
            if (store == null)
            {
                return Result.NotFound("Store Not Found");
            }
            var admin= store.OwnerId;
         
            if (admin == null || admin != userId)
            {
                return Result.BadRequest("This user is not admin in this store");
            }
            product.IsDeleted = true;
            product.DeletedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);         
            return Result.Success();
        }

        public async Task<Result<List<ProductResponse>>> FilterAsync(FilterDto dto )
        {
            var products = await _productRepository.FilterAsync(dto.minPrice, dto.maxPrice, dto.Type.Value,dto.Color,dto.Size);
            if (products == null || products.Count == 0)
            {
                return Result<List<ProductResponse>>.NotFound("No products found for the given filters");
            }
            List<ProductResponse> res = products.Select(p => new ProductResponse
            {
                Id = p.Id,
                Name=p.Name,
                StoreId=p.StoreId,
                CategoryId=p.CategoryId,

                Description = p.Description,
                Price = p.Price,

                Rating = p.Rating,
                Image = p.Image,
                Quantity = p.Quantity,
                DiscountPercentage=p.DiscountPercentage,
                PriceAfterDiscount=p.PriceAfterDiscount

            }).ToList();
            return Result<List<ProductResponse>>.Success(res, "Success");

        }

        public async Task<Result<ProductResponse>> GetProductAsync(int productId)
        {
            var prod=await _productRepository.GetByIdAsync(productId);
            if (prod == null)
            {
                return Result<ProductResponse>.NotFound("Product Not Found");
            }
            var res = new ProductResponse
            {
                Id = prod.Id,
                Description = prod.Description,
                StoreId=prod.StoreId,
                CategoryId=prod.CategoryId,
                Price = prod.Price,
                Rating = prod.Rating,
                Image = prod.Image,
                Quantity = prod.Quantity,
                Name = prod.Name,
                DiscountPercentage=prod.DiscountPercentage,
                DiscountStartDate=prod.DiscountStartDate,
                DiscountEndDate=prod.DiscountEndDate,
                PriceAfterDiscount = prod.PriceAfterDiscount
            };
            return Result<ProductResponse>.Success(res);
            
        }

        public async Task<Result<List<ProductResponse>> >SearchAsync(string Query)
        {
            var products=await _productRepository.SearchAsync(Query);
            if (products == null || products.Count == 0)
            {
                return Result<List <ProductResponse>>.NotFound("No products found for the given filters");
            }
            List <ProductResponse > res = products.Select(p => new ProductResponse 
            {
                Id = p.Id,
                Name = p.Name,
               
                Description = p.Description,
                StoreId=p.StoreId,
                CategoryId=p.CategoryId,
                Price = p.Price,
                DiscountPercentage=p.DiscountPercentage,
                DiscountStartDate=p.DiscountStartDate,
                DiscountEndDate=p.DiscountEndDate,
                PriceAfterDiscount=p.PriceAfterDiscount,

                Rating = p.Rating,
                Image = p.Image,
                Quantity = p.Quantity,



            }).ToList();
            
                

            return Result<List<ProductResponse>>.Success(res,"Success");


        }

        public async Task<Result<ProductResponse>> UpdateAsync(string userId, UpdateProductRequest request,int productId)
        {
            var prod= await _productRepository.GetByIdAsync(productId);
            if (prod == null)
            {
                return Result<ProductResponse>.NotFound("NotFound");
            }
            var store=await _storeRepository.GetByIdAsync(prod.StoreId);
            if (store == null)
            {
                return Result<ProductResponse>.NotFound("Store Not Found");
            }
            var adminId = store.OwnerId;
            if (adminId == null || adminId != userId)
            {
                return Result<ProductResponse>.UnAuthorized("This user is not admin in this store");
            }
            if (request.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync(request.CategoryId.Value);
                if (category == null)
                {
                    return Result<ProductResponse>.NotFound("Category Not Found");
                }
                prod.CategoryId = category.Id;   

            }
            if (request.Image != null)
            {
               var img= await _imageService.UploadImageAsync(request.Image);
                prod.Image = img.Value;
            }
            if (request.Quantity.HasValue)
            {
                if(request.Quantity.Value < 0)
                {
                    return Result<ProductResponse>.BadRequest("Quantity must be greater than or equal to 0");
                }
                prod.Quantity = request.Quantity.Value;
            }

            if (request.Price.HasValue)
            {
                if( request.Price.Value <= 0)
                {
                    return Result<ProductResponse>.BadRequest("Price must be greater than 0");
                }
                prod.Price = request.Price.Value;
            }
            if (request.DiscountPrecentage.HasValue)
            {
                if(request.DiscountPrecentage.Value <0 || request.DiscountPrecentage.Value >100)
                {
                    return Result<ProductResponse>.BadRequest("Discount Precentage has to between 0 and 100");
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
                    return Result<ProductResponse>.BadRequest("Discount end date must be after start date");
            }
            if (prod.DiscountPercentage.HasValue &&
             (!prod.DiscountStartDate.HasValue || !prod.DiscountEndDate.HasValue))
            {
                return Result<ProductResponse>.BadRequest("Discount dates must be provided when setting discount percentage");
            }




            await _productRepository.UpdateAsync(prod);

            var res = new ProductResponse
            {
                Id = prod.Id,
                Name = prod.Name,
                Description = prod.Description,
                StoreId = prod.StoreId,
                CategoryId =prod.CategoryId,
              
                Price = prod.Price,
                Rating = prod.Rating,
                Quantity = prod.Quantity,
                Image = prod.Image,
                DiscountPercentage = prod.DiscountPercentage,
                DiscountStartDate = prod.DiscountStartDate,
                DiscountEndDate = prod.DiscountEndDate,
                PriceAfterDiscount=prod.PriceAfterDiscount
              



            };
            return Result<ProductResponse>.Success(res);
            
        }
    }
}
