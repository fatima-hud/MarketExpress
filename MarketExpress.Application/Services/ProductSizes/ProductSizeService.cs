using MarketExpress.Application.Dto.ProductSize.Request;
using MarketExpress.Application.Dto.ProductSize.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;

namespace MarketExpress.Application.Services.ProductSizes
{
    public class ProductSizeService : IProductSizeService
    {
        private readonly IProductColorRepository _productColorRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly IProductRepository _productRepository;

        public ProductSizeService(IProductColorRepository productColorRepository, IStoreRepository storeRepository, IProductSizeRepository productSizeRepository, IProductRepository productRepository)
        {
            _productColorRepository = productColorRepository;
            _storeRepository = storeRepository;
            _productSizeRepository = productSizeRepository;
            _productRepository = productRepository;
        }

        public async Task<Result<List<ResponseGetProductSizeDto>>> AddAsync(string userId, int ProductColorId, List<RequestProductSizeDto> request)
        {
            var productColor = await _productColorRepository.GetByIdAsync(ProductColorId);
            if (productColor == null)
            {
                return Result<List<ResponseGetProductSizeDto>>.NotFound("المنتج غير موجود");
            }
            var product = await _productRepository.GetByIdAsync(productColor.ProductId);
            if (product == null)
            {
                return Result<List<ResponseGetProductSizeDto>>.NotFound("المنتج غير موجود");
            }


            var store = await _storeRepository.GetByIdAsync(product.StoreId);
            if (store == null)
            {

                return Result<List<ResponseGetProductSizeDto>>.NotFound("المتجر غير موجود");
            }
            var admin = store.OwnerId;
            if (admin == null || admin != userId)
            {

                return Result<List<ResponseGetProductSizeDto>>.BadRequest("المستخدم ليس صاحب المتجر");
            }
            if (request is null)
            {
                return Result<List<ResponseGetProductSizeDto>>.BadRequest("يجب ادخال القياسات ");
            }
            if (request != null && request.Count == 0)
            {

                return Result<List<ResponseGetProductSizeDto>>.BadRequest("يجب ادخال القياسات ");

            }


            if (request.Any(e => e.Quantity <= 0))
            {

                return Result<List<ResponseGetProductSizeDto>>.BadRequest("الكمية يجب ان تكون اكبر من 0");

            }
            var sizes = request.Select(e => e.Size).Distinct().ToList();
            if (sizes.Count != request.Count)
            {
                return Result<List<ResponseGetProductSizeDto>>.BadRequest("يوجد قياسات مكررة");
            }
            foreach (var item in request)
            {
                var exists = await _productSizeRepository.GetProductSizeByProductColorAndSizeAsync(ProductColorId, item.Size);

                if (exists != null)
                {

                    return Result<List<ResponseGetProductSizeDto>>.BadRequest(" المنتج متوفر مسبقا");

                }
            }
            var productSize = request.Select(e => new ProductSize
            {
                ProductColorId = productColor.Id,
                Quantity = e.Quantity,
                Size = e.Size
            }).ToList();
            await _productSizeRepository.AddRangeAsync(productSize);
            var ret = productSize.Select(e => new ResponseGetProductSizeDto
            {
                Size = e.Size,
                Quantity = e.Quantity,
                IsFound = e.IsFoundProduct
            }).ToList();
            return Result<List<ResponseGetProductSizeDto>>.Success(ret);



        }
        public async Task<Result<ResponseGetProductSizeDto>> UpdateQuantityAsync(string userId, int productSizeId, RequestUpdateProductSizeDto request)
        {

            var productSize = await _productSizeRepository.GetByIdAsync(productSizeId);
            if (productSize == null)

            {
                return Result<ResponseGetProductSizeDto>.NotFound("العنصر غير موجود");
            }

            var productId = await _productColorRepository.GetProductByProductColorAsync(productSize.ProductColorId);


            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return Result<ResponseGetProductSizeDto>.NotFound("المنتج غير موجود");
            }
            var store = await _storeRepository.GetByIdAsync(product.StoreId);
            if (store == null)
            {
                return Result<ResponseGetProductSizeDto>.NotFound("المتجر غير موجود");
            }
            var admin = store.OwnerId;
            if (admin == null || admin != userId)
            {
                return Result<ResponseGetProductSizeDto>.BadRequest("المستخدم ليس صاحب المتجر");
            }








            if (request.Quantity <= 0)
            {
                return Result<ResponseGetProductSizeDto>
                    .BadRequest("الكمية يجب ان تكون اكبر من 0");
            }

            productSize.Quantity = request.Quantity;


            await _productSizeRepository.UpdateAsync(productSize);

            var res = new ResponseGetProductSizeDto
            {

                Size = productSize.Size,
                Quantity = productSize.Quantity,
                IsFound = productSize.IsFoundProduct

            };
            return Result<ResponseGetProductSizeDto>.Success(res);


        }


    }
}
