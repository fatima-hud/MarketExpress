using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Delivery.Request;
using MarketExpress.Application.Dto.Delivery.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Application.Implementations
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<User> _userManager;
        private readonly IAesEncryptionService _aesEncryptionService;

        public DeliveryService(IDeliveryRepository deliveryRepository, IOrderRepository orderRepository, UserManager<User> userManager,IAesEncryptionService aesEncryptionService)
        {
            _deliveryRepository = deliveryRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _aesEncryptionService = aesEncryptionService;
        } 

        public async Task<Result<ResponseDeliveryDto>> AddDeliveryAsync(string dispatcherId, RequestAddDeliveryDto request)
        {
            var dispatcher = await _userManager.FindByIdAsync(dispatcherId);
            if (dispatcher == null)
            {
                return Result<ResponseDeliveryDto>.NotFound("Dispatcher not found");
            }
            var dispatcherRole = await _userManager.IsInRoleAsync(dispatcher, "Dispatcher");
            if (!dispatcherRole)
            {
                return Result<ResponseDeliveryDto>.BadRequest("User is not a dispatcher");
            }
            var driver = await _userManager.FindByIdAsync(request.DriverId);
            if (driver == null)
            {
                return Result<ResponseDeliveryDto>.NotFound("Driver not found");
            }
            var driverRole = await _userManager.IsInRoleAsync(driver, "Driver");
            if (!driverRole)
            {
                return Result<ResponseDeliveryDto>.BadRequest("User is not a driver");

            }
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                return Result<ResponseDeliveryDto>.NotFound("Order not found");
            }
            if (order.Status != enOrderStatus.Processing)
            {
                return Result<ResponseDeliveryDto>.BadRequest("Order is not in processing state");
            }
            var eOrder = _aesEncryptionService.Encrypt(order.Address);

            var delivery = new Delivery
            {
                DriverId = request.DriverId,
                OrderId = request.OrderId,
                StartTime = DateTime.UtcNow,
                statue = enDeliveryStatue.Pending,
                AddressLocation = eOrder

            };
            await _deliveryRepository.AddAsync(delivery);
            var dOrder = _aesEncryptionService.Decrypt(delivery.AddressLocation);
            var res = new ResponseDeliveryDto
            {
                Id = delivery.Id,
                DriverId = delivery.DriverId,
                DriverName = driver.FullName,
                DriverPhoneNumber = driver.PhoneNumber,
                OrderId = delivery.OrderId,
                StartTime = delivery.StartTime,
                EndTime = delivery.EndTime,
                Status = delivery.statue,
                AddressLocation = dOrder
            };
            return Result<ResponseDeliveryDto>.Success(res);
        }


        public async Task<Result<ResponseDeliveryDto>> GetDeliveryAsync(string UserId, int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                return Result<ResponseDeliveryDto>.NotFound("Order not found");
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return Result<ResponseDeliveryDto>.NotFound("User not found");

            }
            if (order.UserId != user.Id)
            {
                return Result<ResponseDeliveryDto>.BadRequest("User is not the owner of the order");
            }
            var delivery = await _deliveryRepository.GetDeliveryByOrderIdAsync(orderId);
            if (delivery == null)
            {
                return Result<ResponseDeliveryDto>.NotFound("Delivery not found");
            }
            var driver = await _userManager.FindByIdAsync(delivery.DriverId);
            if (driver == null)
            {
                return Result<ResponseDeliveryDto>.NotFound("Driver is not found");
            }
            var dOrder = _aesEncryptionService.Decrypt(delivery.AddressLocation);
            var ret = new ResponseDeliveryDto
            {
                Id = delivery.Id,
                DriverId = delivery.DriverId,
                DriverName = driver.FullName,
                DriverPhoneNumber = driver.PhoneNumber,
                OrderId = delivery.OrderId,
                StartTime = delivery.StartTime,
                EndTime = delivery.EndTime,
                Status = delivery.statue,
                AddressLocation = dOrder
            };
            return Result<ResponseDeliveryDto>.Success(ret);
        }
    }
}
