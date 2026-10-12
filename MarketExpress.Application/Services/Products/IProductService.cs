using MarketExpress.Application.Dto.Product.Request;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Core.DtoHelping.Admin.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.Products
{
    public interface IProductService
    {
        Task<Result<List<ResponseGetDiscountProductDto>>> SearchAsync(string Query);
        Task<Result<List<ResponseGetDiscountProductDto>>> FilterAsync(FilterDto dto);
        Task<Result<ResponseProductDetailsDto>> AddAsync(string userId, RequestAddProductDto request);
        Task<Result<ResponseUpdateProductDto>> UpdateAsync(string userId, RequestUpdateProductDto request, int productId);
        Task<Result> DeleteAsync(string userId, int id);
        Task<Result<ResponseProductDetailsDto>> GetProductAsync(int productId);
        Task<Result<List<ResponseGetDiscountProductByStoreDto>>> GetAllDiscountProductByStore(string userId);
        Task<Result<List<ResponseGetDiscountProductDto>>> GetAllDiscountProductByAllStores();
        Task<Result<ResponseProductDashboardDto>> GetProductDashboardsAsync(string userId,int PageNumber, int PageSize);




    }
}
