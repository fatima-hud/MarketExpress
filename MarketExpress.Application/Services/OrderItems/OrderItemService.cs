using MarketExpress.Application.Dto.Order.Response;
using MarketExpress.Core.DtoHelping.ProductInventory;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Application.Services.OrderItems
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly UserManager<User> _userManager;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStoreRepository _storeRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository, UserManager<User> userManager, IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository, IStoreRepository storeRepository)
        {
            _orderItemRepository = orderItemRepository;
            _userManager = userManager;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _storeRepository = storeRepository;
        }
        public async Task<Result<List<ResponseOrderItemDto>>> GetOrderDetails(string userId, int orderId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<List<ResponseOrderItemDto>>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<List<ResponseOrderItemDto>>.BadRequest("مستخدم محظور");
            }
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return Result<List<ResponseOrderItemDto>>.NotFound("الطلب غير موجود");
            }
            var userRoles = await _userManager.GetRolesAsync(user);

            var isSuperAdmin = userRoles.Contains("SuperAdmin");
            if (order.UserId != userId && !isSuperAdmin)
            {
                return Result<List<ResponseOrderItemDto>>.BadRequest("غير مصرح لك الوصول لهذا الطلب");
            }
            var orderItems = await _orderItemRepository.GetOrderItemsAsync(orderId);

            var ret = orderItems.Select(e => new ResponseOrderItemDto
            {
                Id = e.Id,
                UserId = order.UserId,
                OrderId = orderId,
                ProductId = e.ProductId,
                Quantity = e.Quantity,
                Price = e.Price,
                Color = e.Color,
                ColorHex = e.ColorHexCode,
                Size = e.Size,
                Image =
        e.product.ProductColors
        .FirstOrDefault(c => c.Color == e.Color)
        ?.Image

        ?? e.product.Image



            }).ToList();
            return Result<List<ResponseOrderItemDto>>.Success(ret);

        }

        public async Task<Result<List<ResponseProductInventoryDto>>> GetProductInventoryAsync(string userId, DateTime? startDate, DateTime? endDate)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<List<ResponseProductInventoryDto>>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<List<ResponseProductInventoryDto>>.BadRequest("مستخدم محظور");
            }
            var store = await _storeRepository.GetStoreByAdminAsync(userId);
            if (store == null)
            {
                return Result<List<ResponseProductInventoryDto>>.NotFound("المتجر غير موجود");
            }
            if (!startDate.HasValue || !endDate.HasValue)
            {
                return Result<List<ResponseProductInventoryDto>>
                    .BadRequest("تاريخ غير صالح");
            }
            if (startDate > endDate)
            {
                return Result<List<ResponseProductInventoryDto>>.BadRequest("يجب ان يكون تاريخ بداية الجرد اصغر من تاريخ النهاية");
            }
            var res = await _orderItemRepository.GetProductInventoryAsync(store.Id, startDate.Value, endDate.Value);
            if (res == null)
            {
                return Result<List<ResponseProductInventoryDto>>.NotFound("لا يوجد مبيعات في هذا المتجر خلال الفترة المحددة");

            }
            return Result<List<ResponseProductInventoryDto>>.Success(res);
        }
    }

}
