using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Core.DtoHelping.Admin.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace MarketExpress.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepositoryAsync<Product>, IProductRepository
    {


        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {


        }

        public async Task<List<ResponseGetDiscountProductDto>> FilterAsync(
    enType? Type,
    string? Color,
    enSize? Size,
    decimal? minPrice,
    decimal? maxPrice)
        {
            var now = DateTime.Now;

            IQueryable<Product> query = _dbContext.Products
                .Include(p => p.ProductColors)
                .ThenInclude(c => c.ProductSizes);

            // Type
            if (Type.HasValue)
            {
                query = query.Where(p => p.Type == Type.Value);
            }

            // Color
            if (!string.IsNullOrEmpty(Color))
            {
                query = query.Where(p =>
                    p.ProductColors.Any(c => c.Color == Color));
            }

            // Size
            if (Size.HasValue)
            {
                query = query.Where(p =>
                    p.ProductColors.Any(c =>
                        c.ProductSizes.Any(s => s.Size == Size.Value)));
            }

            // Min Price
            if (minPrice.HasValue)
            {
                query = query.Where(p =>
                    (
                        p.DiscountPercentage.HasValue &&
                        p.DiscountPercentage.Value > 0 &&
                        p.DiscountStartDate <= now &&
                        p.DiscountEndDate >= now
                            ? p.Price -
                              (p.Price * p.DiscountPercentage.Value / 100m)
                            : p.Price
                    ) >= minPrice.Value);
            }

            // Max Price
            if (maxPrice.HasValue)
            {
                query = query.Where(p =>
                    (
                        p.DiscountPercentage.HasValue &&
                        p.DiscountPercentage.Value > 0 &&
                        p.DiscountStartDate <= now &&
                        p.DiscountEndDate >= now
                            ? p.Price -
                              (p.Price * p.DiscountPercentage.Value / 100m)
                            : p.Price
                    ) <= maxPrice.Value);
            }

            var products = await query
                .Select(p => new ResponseGetDiscountProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    StoreId = p.StoreId,

                    Price = p.Price,

                    DiscountPercentage = p.DiscountPercentage,
                    DiscountStartDate = p.DiscountStartDate,
                    DiscountEndDate = p.DiscountEndDate,

                    PriceAfterDiscount =
                        p.DiscountPercentage.HasValue &&
                        p.DiscountPercentage.Value > 0 &&
                        p.DiscountStartDate <= now &&
                        p.DiscountEndDate >= now
                            ? p.Price -
                              (p.Price * p.DiscountPercentage.Value / 100m)
                            : p.Price,

                    Image = string.IsNullOrEmpty(Color)
                        ? p.Image
                        : p.ProductColors
                            .Where(c => c.Color == Color)
                            .Select(c => c.Image)
                            .FirstOrDefault() ?? p.Image
                })
                .ToListAsync();

            return products;
        }
        public async Task<List<Product>> SearchAsync(string query)
        {
            var terms = query
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            IQueryable<Product> products = _dbContext.Products
                .Include(e => e.ProductColors).ThenInclude(e => e.ProductSizes);

            // النوع
            var parsedType = terms
                .FirstOrDefault(t => Enum.TryParse<enType>(t, true, out _));

            if (parsedType != null)
            {
                Enum.TryParse(parsedType, true, out enType type);

                products = products.Where(p => p.Type == type);
            }

            // القياس
            var parsedSize = terms
                .FirstOrDefault(t => Enum.TryParse<enSize>(t, true, out _));

            if (parsedSize != null)
            {
                Enum.TryParse(parsedSize, true, out enSize size);

                products = products.Where(p =>
                    p.ProductColors.Any(e => e.ProductSizes.Any(s => s.Size == size)));
            }

            // اللون
            var colors = terms.Where(t =>
                !Enum.TryParse<enType>(t, true, out _) &&
                !Enum.TryParse<enSize>(t, true, out _));

            foreach (var color in colors)
            {
                products = products.Where(p =>
                    p.ProductColors.Any(c =>
                        c.Color.ToLower().Contains(color)

                        ||

                        p.Name.ToLower().Contains(color)

                        ||

                        p.Description.ToLower().Contains(color)
                    ));
            }

            return await products.ToListAsync();
        }
        public async Task<bool> IsProductHasSizeAsync(int productColorId, enSize size)
        {
            var product = await _dbContext.ProductColors.Include(e => e.ProductSizes).FirstOrDefaultAsync(e => e.Id == productColorId);
            if (product == null)
                return false;
            return product.ProductSizes.Any(c => c.Size == size);
        }
        public async Task<bool> IsProductHasColorAsync(int productId, string Color)
        {
            var product = await _dbContext.Products.Include(e => e.ProductColors).FirstOrDefaultAsync(e => e.Id == productId);
            if (product == null)
                return false;
            return product.ProductColors.Any(c => c.Color.ToLower() == Color.ToLower());
        }

        public async Task<double> GetRatingAsync(int productId)
        {
            var ratings = await _dbContext.Ratings
        .Where(e => e.ProductId == productId && e.RatingValue.HasValue)
        .Select(e => e.RatingValue.Value)
        .ToListAsync();
            if (ratings.Count == 0)
                return 0;
            return ratings.Average();

        }

        public async Task<Store> GetStoreByProductIdAsync(int ProductId)
        {
            var res = await _dbContext.Products.Include(e => e.Store)
                .Where(e => e.Id == ProductId)
                .Select(e => e.Store)
                .FirstOrDefaultAsync();
            return res;
        }

        public async Task<List<Product>> GetAllProductByStoreAsync(int storeId)
        {
            var res = await _dbContext.Products.Where(e => e.StoreId == storeId ).ToListAsync();
            return res;
        }

        public async Task<List<Product>> GetAllDiscountProductsByStoreAsync(int storeId)
        {
            var dayDate = DateTime.UtcNow;
            var res = await _dbContext.Products.Where(e => e.StoreId == storeId && e.DiscountPercentage > 0 && e.DiscountStartDate <= dayDate && e.DiscountEndDate > dayDate).ToListAsync();
            return res;
        }

        public async Task<List<Product>> GetAllDiscountProductsByAllStoresAsync()
        {
            var dayDate = DateTime.UtcNow;
            var res = await _dbContext.Products.Where(e => e.DiscountPercentage > 0 && e.DiscountStartDate <= dayDate && e.DiscountEndDate > dayDate).ToListAsync();
            return res;
        }

        public async Task<ResponseProductDashboardDto> GetProductDashboardsAsync(int storeId,int pageNumber,int pageSize)
        {
            var now = DateTime.UtcNow;
            var productsCount = await _dbContext.Products.AsNoTracking().Where(p => p.StoreId == storeId).CountAsync();
        
            var products = await _dbContext.Products.AsNoTracking().Where(p => p.StoreId == storeId).OrderByDescending(p => p.Id)
    .Skip(pageSize * (pageNumber - 1))
    .Take(pageSize).Select(p => new ResponseGetProductsByAdminDto
            {
                Id = p.Id,
                Name = p.Name,
                CategoryId = p.CategoryId,
                Description = p.Description,
                Price = p.Price,
                DiscountPercentage = p.DiscountPercentage,
                DiscountStartDate = p.DiscountStartDate,
                DiscountEndDate = p.DiscountEndDate,
       PriceAfterDiscount= p.DiscountPercentage.HasValue &&
                p.DiscountPercentage.Value > 0 &&
                p.DiscountStartDate <= now &&
                p.DiscountEndDate >= now
                    ? p.Price -
                      (p.Price * p.DiscountPercentage.Value / 100m)
                    : p.Price,

        RatingValue =
                 p.Ratings.Any(r => r.RatingValue.HasValue)
               ? p.Ratings
            .Where(r => r.RatingValue.HasValue)
            .Average(r => r.RatingValue!.Value)
        : 0,
                Image = p.Image,
                Colors = p.ProductColors.Select(c => new ResponseGetAllColorByAdminDto
                {
                    ProductColorId=c.Id,
                    Color = c.Color,
                    ColorHexCode = c.ColorHexCode,
                    Sizes = c.ProductSizes.Select(s => new ResponseGetAllSizeByAdminDto
                    {
                        ProductSizeId = s.Id,
                        Size = s.Size,
                        IsFoundProduct = s.IsFoundProduct,
                        Quantity = s.Quantity,
                    }).ToList(),
                   

                }).ToList(),
                SoldCount = p.OrderItems.Where(o=>o.order.Status==enOrderStatus.Delivered).Sum(o => o.Quantity),
                SoldTotalPrice = p.OrderItems.Where(o => o.order.Status == enOrderStatus.Delivered).Sum(o => o.Price*o.Quantity),
                TotalStock = p.ProductColors
          .SelectMany(c => c.ProductSizes)
           .Sum(s => s.Quantity)








            }).ToListAsync();
            var ret = new ResponseProductDashboardDto
            {
                Products = products,
                TotalProductsCount = productsCount
            };
            return ret;
        }

        public async Task<ResponseDashboardSummaryDto> GetDashboardSummaryAsync(int storeId)
        {
            var products = await _dbContext.Products.AsNoTracking().Where(p => p.StoreId == storeId).CountAsync();
            var followers = await _dbContext.StoreFollowers.AsNoTracking().Where(f => f.StoreId == storeId && f.IsFollow).CountAsync();
            var posts = await _dbContext.Posts.AsNoTracking().Where(p => p.StoreId == storeId).CountAsync();
            var totalReactions=await _dbContext.PostReactions.AsNoTracking().Where(pr=>pr.Post.StoreId==storeId && pr.IsReacted).CountAsync();
            var ret = new ResponseDashboardSummaryDto
            {
                ProductsCount = products,
                FollowersCount = followers,
                PostsCount = posts,
                TotalReactions = totalReactions
            };
            return ret;
        }

        public async Task<ResponseDashboardAnalyticsDto> GetDashboardAnalyticsAsync(
      int storeId,
      DateTime fromDate,
      DateTime endDate)
        {
            var from = fromDate.Date;
            var to = endDate.Date.AddDays(1);

            var orderItems = _dbContext.Orderitems
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Where(oi =>
                    oi.product.StoreId == storeId &&
                    oi.order.Status == enOrderStatus.Delivered &&
                    oi.order.CreatedAt >= from &&
                    oi.order.CreatedAt < to);

            var ordersCount = await orderItems
                .GroupBy(o => o.OrderId)
                .CountAsync();

            var totalSales = await orderItems
                .SumAsync(o => o.Price * o.Quantity);

            var customerCount = await orderItems
                .GroupBy(oi => oi.order.UserId)
                .CountAsync();

            return new ResponseDashboardAnalyticsDto
            {
                OrdersCount = ordersCount,
                TotalSales = totalSales,
                CustomersCount = customerCount
            };
        }

    }
}





