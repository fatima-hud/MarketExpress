using MarketExpress.Application.Dto.ProductSize.Request;
using MarketExpress.Application.Dto.ProductSize.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.ProductSizes
{
    public interface IProductSizeService
    {
        Task<Result<List<ResponseGetProductSizeDto>>> AddAsync(string userId, int ProductColorId, List<RequestProductSizeDto> request);
        Task<Result<ResponseGetProductSizeDto>> UpdateQuantityAsync(string userId, int productSize, RequestUpdateProductSizeDto request);
    }
}
