using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Core.DtoHelping.Admin.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Infrastructure.InfrastructureBases;
using System.Security.Cryptography;

namespace MarketExpress.Infrastructure.Abstacts
{
    public interface IProductRepository : IGenericRepositoryAsync<Product>
    {
        Task<List<Product>> SearchAsync(string Query);
        Task<List<ResponseGetDiscountProductDto>> FilterAsync(enType? Type, string? Color, enSize? Size, decimal? minPrice, decimal? maxPrice);

        Task<bool> IsProductHasSizeAsync(int productId, enSize size);
        Task<bool> IsProductHasColorAsync(int productId, string Color);
        Task<double> GetRatingAsync(int productId);
        Task<Store> GetStoreByProductIdAsync(int productId);
        Task<List<Product>> GetAllProductByStoreAsync(int storeId);
        Task<List<Product>> GetAllDiscountProductsByStoreAsync(int storeId);
        Task<List<Product>> GetAllDiscountProductsByAllStoresAsync();
        Task<ResponseProductDashboardDto> GetProductDashboardsAsync(int storeId,int pageNumber,int pageSize);
        Task<ResponseDashboardSummaryDto> GetDashboardSummaryAsync(int storeId);
        Task<ResponseDashboardAnalyticsDto>GetDashboardAnalyticsAsync(int storeId, DateTime FromDate,DateTime EndDate);





    }
}
