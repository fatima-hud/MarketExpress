using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Checkout.Response;
using MarketExpress.Application.Dto.Order.Request;
using MarketExpress.Application.Dto.Order.Response;
using MarketExpress.Application.Dto.Payment.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MarketExpress.Application.Implementations
{
    public class CheckoutService : ICheckoutService
    {
        private readonly UserManager<User> _userManager;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IStoreRepository _storeRepository;
     private readonly ITransactionRepository _transactionRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IClothingItemRepository _clothingItemRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAesEncryptionService _aesEncryptionService;

        public CheckoutService(UserManager<User> userManager, ICartRepository cartRepository, ICartItemRepository cartItemRepository,  IOrderRepository orderRepository, IOrderItemRepository orderItemRepository,IClothingItemRepository clothingItemRepository,IWalletRepository walletRepository,IStoreRepository storeRepository,IPaymentRepository paymentRepository,ITransactionRepository transactionRepository,IAesEncryptionService aesEncryptionService)
        {
            _userManager = userManager;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _clothingItemRepository = clothingItemRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _walletRepository = walletRepository;
            _storeRepository = storeRepository;
            _paymentRepository = paymentRepository;
            _transactionRepository = transactionRepository;
            _aesEncryptionService = aesEncryptionService;
        }

        public async Task<Result<ResponseCancelOrderDto>> CancelOrderAsync(string userId, int orderId)
        {
            var trans = await _orderRepository.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    await trans.RollbackAsync();
                    return Result < ResponseCancelOrderDto >.NotFound("المستخدم غير موجود");
                }
                if (user.IsBanned)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseCancelOrderDto>.BadRequest("المستخدم محظور");
                }
               var order =await _orderRepository.GetByIdAsync(orderId);
                if (order == null)
                {

                    await trans.RollbackAsync();
                    return Result<ResponseCancelOrderDto>.NotFound("الطلب غير موجود");
                }
                if (order.UserId != userId)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseCancelOrderDto>.BadRequest("المستخدم ليس صاحب الطلب");
                }
                if (order.Status != enOrderStatus.Processing) {
                    await trans.RollbackAsync();
                    return Result<ResponseCancelOrderDto>.BadRequest("لايمكن الغاء الطلب");
                }

                var orderItems = await _orderItemRepository.GetOrderItemsAsync(orderId);
                if (!orderItems.Any())
                {
                    await trans.RollbackAsync();
                    return Result<ResponseCancelOrderDto>.NotFound("لا يوجد عناصر للطلب");

                }
                var totalPrice = order.TotalPrice;
             
                var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
                if (wallet is null)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseCancelOrderDto>.NotFound("Wallet not found");
                }
              
                wallet.Balance += totalPrice;
                await _walletRepository.SaveChangesAsync();
                var groupedItems = await _orderItemRepository.GroupingItemsForStoresAsync(order.Id);
                foreach (var storeGroup in groupedItems)
                {
                    var total = storeGroup.Sum(x => x.Quantity * x.product.PriceAfterDiscount);

                    var storeWallet = await _storeRepository.GetWalletByStoreIdAsync(storeGroup.Key);
                    if (storeWallet is null)
                    {
                        await trans.RollbackAsync();
                        return Result<ResponseCancelOrderDto>.NotFound("محفظة المتجر غير موجودة");
                    }

                    storeWallet.Balance -= total;
                }

                foreach (var item in orderItems)
                {
                    var stockItem = await _clothingItemRepository
                        .GetClothingItemByProductAsync(item.ProductId, item.Size, item.Color);

                    stockItem.Quantity += item.Quantity;
                }
                await _clothingItemRepository.SaveChangesAsync();
                order.Status =enOrderStatus.Cancelled;
                var payment= await _paymentRepository.GetPaymentByOrderIdAsync(orderId);
                if (payment == null)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseCancelOrderDto>.NotFound("سجل الدفع غير موجود");
                }
               payment.Status =enPaymentStatus.Cancelled;
                await _paymentRepository.SaveChangesAsync();
                var userTransaction = new TransactionProcess
                {
                    WalletId = wallet.Id,
                    PaymentId = payment.Id,
                    Amount = payment.Amount,
                    TransactionType = enTransactionType.Deposit,
                    Date = DateTime.UtcNow

                };
                await _transactionRepository.AddAsync(userTransaction);
                foreach (var storeGroup in groupedItems)
                {
                    var total = storeGroup.Sum(x => x.Quantity * x.product.PriceAfterDiscount);

                    var storeWallet = await _storeRepository.GetWalletByStoreIdAsync(storeGroup.Key);
                    if (storeWallet is null)
                    {
                        await trans.RollbackAsync();
                        return Result<ResponseCancelOrderDto>.NotFound("محفظة المتجر غير موجودة");
                    }

                    var adminTransaction = new TransactionProcess
                    {
                        WalletId = storeWallet.Id,
                        PaymentId = payment.Id,
                        Amount = total,
                        Date = userTransaction.Date,
                        TransactionType = enTransactionType.Withdraw
                    };
                    await _transactionRepository.AddAsync(adminTransaction);
                }

               

                var responseOrder = new ResponseAddOrderDto
                {
                    Id = order.Id,
                    Address = order.Address,
                    UserId = userId,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    CreatedAt = order.CreatedAt,
                    Status = order.Status,
                    TotalPrice = order.TotalPrice
                };
             
                var responsePayment = new ResponseGetPaymentDto
                {
                    Id = payment.Id,
                    Amount = payment.Amount,
                    OrderId = payment.OrderId,
                    Date = payment.Date,
                    WalletId = payment.WalletId,
                    Status = payment.Status
                };

                var res = new ResponseCancelOrderDto
                {
                    Order = responseOrder,
                   
                    Payment = responsePayment

                };

               
                await trans.CommitAsync();
                return Result<ResponseCancelOrderDto>.Success(res);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<ResponseCancelOrderDto>.Failure($"خطا داخلي {ex.Message}");



            }
        }

    
        public async Task<Result<CheckoutDto>> CheckoutAsync(string userId, RequestAddOrderDto request)
        {
            var trans = await _orderRepository.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    await trans.RollbackAsync();
                    return Result<CheckoutDto>.NotFound("المستخدم غير موجود");
                }
                if (user.IsBanned)
                {
                    await trans.RollbackAsync();
                    return Result<CheckoutDto>.BadRequest("المستخدم محظور");
                }
                var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    await trans.RollbackAsync();
                    return Result<CheckoutDto>.NotFound("السلة غير موجودة");
                }
                var cartItems = await _cartItemRepository.GetCartItemsByCartIdAsync(cart.Id);
                if (cartItems.Count == 0)
                {
                    await trans.RollbackAsync();
                    return Result<CheckoutDto>.BadRequest("السلة فارغة");
                }
                var totalPrice = cartItems.Sum(e =>
                 e.Product.PriceAfterDiscount * e.Quantity
             );
              
                foreach (var item in cartItems)
                {
                     var stockItem = await _clothingItemRepository.GetClothingItemByProductAsync(item.ProductId, item.Size, item.Color);

                    if (stockItem == null || stockItem.Quantity < item.Quantity)
                    {
                        await trans.RollbackAsync();
                        return Result<CheckoutDto>.BadRequest( $"الكمية المطلوبةاعلى من المخزون  {item.ProductId} "
                           
                        );
                    }

                }

                var wallet = await _walletRepository.GetWalletByUserIdAsync(userId);
                if (wallet is null)
                {
                    await trans.RollbackAsync();
                    return Result<CheckoutDto>.NotFound("Wallet not found");
                }
                if (wallet.Balance < totalPrice)
                {
                    await trans.RollbackAsync();
                    return Result<CheckoutDto>.NotFound("Insufficient balance");
                }
                wallet.Balance -= totalPrice;
                await _walletRepository.SaveChangesAsync();
                var groupedItems = await _cartItemRepository.GroupingItemsForStoresAsync(cart.Id);
                foreach (var storeGroup in groupedItems)
                {
                    var total = storeGroup.Sum(x =>x.Quantity * x.Product.PriceAfterDiscount);
       
                    var storeWallet = await _storeRepository.GetWalletByStoreIdAsync(storeGroup.Key);
                    if(storeWallet is null)
                    {
                        await trans.RollbackAsync();
                        return Result<CheckoutDto>.NotFound("محفظة المتجر غير موجودة");
                    }
                    
                    storeWallet.Balance += total;
                }
               // var aes = new AesEncryptionService();

                var address = _aesEncryptionService.Encrypt(request.Address);

               
                var order = new Order
                {
                    Address = address,
                    CreatedAt = DateTime.UtcNow,
                    Status = enOrderStatus.Processing,
                    TotalPrice = totalPrice,
                    UserId = userId
                };
                await _orderRepository.AddAsync(order);
                var payment = new Payment
                {
                    OrderId = order.Id,
                    Amount = totalPrice,
                    Date = DateTime.UtcNow,
                    WalletId = wallet.Id,
                    Status= enPaymentStatus.Paid

                };
                await _paymentRepository.AddAsync(payment);
                await _paymentRepository.SaveChangesAsync();
                var userTransaction = new TransactionProcess
                {
                    WalletId = wallet.Id,
                    PaymentId = payment.Id,
                    TransactionType = enTransactionType.Withdraw,
                    Amount = totalPrice,
                    Date = DateTime.UtcNow,

                };
                await _transactionRepository.AddAsync(userTransaction);
                
                foreach (var storeGroup in groupedItems)
                {
                    var total = storeGroup.Sum(x => x.Quantity * x.Product.PriceAfterDiscount);

                    var storeWallet = await _storeRepository.GetWalletByStoreIdAsync(storeGroup.Key);
                    if (storeWallet is null)
                    {
                        await trans.RollbackAsync();
                        return Result<CheckoutDto>.NotFound("محفظة المتجر غير موجودة");
                    }
                    var adminTransaction = new TransactionProcess
                    {
                        WalletId = storeWallet.Id,
                        Amount = total,
                        PaymentId = payment.Id,
                        TransactionType = enTransactionType.Deposit,
                        Date = userTransaction.Date

                    };
                    await _transactionRepository.AddAsync(adminTransaction);
                }

                foreach (var item in cartItems)
                {
                    var stockItem = await _clothingItemRepository
                        .GetClothingItemByProductAsync(item.ProductId, item.Size, item.Color);

                    stockItem.Quantity -= item.Quantity;
                }
                await _clothingItemRepository.SaveChangesAsync();
                var orderItems = cartItems.Select(e => new OrderItem
                {
                    ProductId = e.ProductId,
                    Color = e.Color,
                    Size = e.Size,
                    ColorHexCode = e.ColorHexCode,
                    Price = e.Product.PriceAfterDiscount,
                    OrderId = order.Id,
                    Quantity = e.Quantity


                }).ToList();

                await _orderItemRepository.AddRangeAsync(orderItems);

                var dOrder = _aesEncryptionService.Decrypt(order.Address);
                var responseOrder = new ResponseAddOrderDto
                {
                    Id = order.Id,
                    Address = dOrder,
                    UserId = userId,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    CreatedAt = order.CreatedAt,
                    Status= order.Status,
                    TotalPrice = order.TotalPrice
                };
                var responseOrderItems = orderItems.Select(e => new ResponseOrderItemDto
                {
                    Id = e.Id,
                    UserId = userId,
                    ProductId = e.ProductId,
                    Price = e.Price,
                    Quantity = e.Quantity,
                    Color = e.Color,
                    Size = e.Size,
                    ColorHex = e.ColorHexCode,
                    OrderId = e.OrderId


                }).ToList();
                var responsePayment = new ResponseGetPaymentDto
                {
                    Id = payment.Id,
                    Amount = payment.Amount,
                    OrderId = payment.OrderId,
                    Date = payment.Date,
                    WalletId = payment.WalletId,
                    Status = payment.Status
                };

                var res = new CheckoutDto
                {
                    Order = responseOrder,
                    Items = responseOrderItems,
                    Payment = responsePayment
   
                };

                await _cartItemRepository.DeleteRangeAsync(cartItems);
                await trans.CommitAsync();
                return Result<CheckoutDto>.Success(res);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<CheckoutDto>.Failure($"خطا داخلي {ex.Message}");



            }
        }

    }

}
