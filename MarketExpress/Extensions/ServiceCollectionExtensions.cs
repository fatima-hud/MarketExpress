using MarketExpress.Application.Services.AesEncryption;
using MarketExpress.Application.Services.AesEncryptions;
using MarketExpress.Application.Services.Auths;
using MarketExpress.Application.Services.CartItems;
using MarketExpress.Application.Services.Carts;
using MarketExpress.Application.Services.Categories;
using MarketExpress.Application.Services.Checkouts;
using MarketExpress.Application.Services.Comments;
using MarketExpress.Application.Services.Complaints;
using MarketExpress.Application.Services.Deliveries;
using MarketExpress.Application.Services.Emails;
using MarketExpress.Application.Services.Images;
using MarketExpress.Application.Services.Messages;
using MarketExpress.Application.Services.Notifications;
using MarketExpress.Application.Services.OrderItems;
using MarketExpress.Application.Services.Orders;
using MarketExpress.Application.Services.OtpCodes;
using MarketExpress.Application.Services.Payments;
using MarketExpress.Application.Services.Products;
using MarketExpress.Application.Services.ProductColors;
using MarketExpress.Application.Services.ProductSizes;
using MarketExpress.Application.Services.Ratings;
using MarketExpress.Application.Services.StoreCategories;
using MarketExpress.Application.Services.StoreRequests;
using MarketExpress.Application.Services.Stores;
using MarketExpress.Application.Services.SuperAdmins;
using MarketExpress.Application.Services.Transactions;
using MarketExpress.Application.Services.UserNotifications;
using MarketExpress.Application.Services.Users;
using MarketExpress.Application.Services.Wallets;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.Abstracts;
using MarketExpress.Infrastructure.Repositories;
using MarketExpress.Application.Services.Admin;
using MarketExpress.Application.Services.Posts;
using MarketExpress.Application.Services.Videos;
using MarketExpress.Application.Services.StoreFollowers;
using MarketExpress.Application.Services.PostReactions;
namespace MarketExpress.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
           
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOtpCodeRepository, OtpCodeRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartItemService, CartItemService>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<ICheckoutService, CheckoutService>();
          
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<IComplaintRepository, ComplaintRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IComplaintService, ComplaintService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISuperAdminService, SuperAdminService>();
            services.AddScoped<IStoreRequestService, StoreRequestService>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
           // services.AddScoped<INotificationRepository, NotificationRepository>();
           // services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserNotificationService, UserNotificationService>();
           // services.AddScoped<IDeliveryRepository, DeliveryRepository>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<IDeliveryLocationService, DeliveryLocationService>();
          //  services.AddScoped<IDeliveryLocationRepository, DeliveryLocationRepository>();
           services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOtpCodeService, OtpCodeService>();
           
            services.AddScoped<IProductColorService, ProductColorService>();
            services.AddScoped<IProductColorRepository, ProductColorRepository>();
            services.AddScoped<IProductSizeRepository, ProductSizeRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IStoreCategoryService, StoreCategoryService>();
            services.AddScoped<IStoreCategoryRepository, StoreCategoryRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IAesEncryptionService, AesEncryptionService>();
            services.AddScoped<IProductSizeService, ProductSizeService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostReactionRepository, PostReactionRepository>();
            services.AddScoped<IStoreFollowerRepository, StoreFollowerRepository>();
            services.AddScoped<IPostMediaRepository, PostMediaRepository>();
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<IStoreFollowerService, StoreFollowerService>();
            services.AddScoped<IPostReactionService, PostReactionService>();

        }
    }
}
