using MarketExpress.Application.Dto.DeliveryLocation.Request;
using MarketExpress.Application.Dto.DeliveryLocation.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Application.Services.Deliveries
{
    public class DeliveryLocationService : IDeliveryLocationService
    {
        private readonly IDeliveryLocationRepository _deliveryLocationRepository;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly UserManager<User> _userManager;
        private readonly IOrderRepository _orderRepository;

        public DeliveryLocationService(IDeliveryLocationRepository deliveryLocationRepository, IDeliveryRepository deliveryRepository, UserManager<User> userManager, IOrderRepository orderRepository)
        {
            _deliveryLocationRepository = deliveryLocationRepository;
            _deliveryRepository = deliveryRepository;
            _userManager = userManager;
            _orderRepository = orderRepository;
        }

        public async Task<Result<ResponseDeliveryLocationDto>> AddAsync(RequestAddDeliveryLocationDto request)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(request.DeliveyId);
            if (delivery is null)
            {
                return Result<ResponseDeliveryLocationDto>.NotFound("Delivery not found");
            }
            if (request.Latitude < -90 || request.Latitude > 90)
                return Result<ResponseDeliveryLocationDto>.BadRequest("Invalid latitude");

            if (request.Longitude < -180 || request.Longitude > 180)
                return Result<ResponseDeliveryLocationDto>.BadRequest("Invalid longitude");
            var deliveryLocation = new DeliveryLocation
            {
                DeliveryId = delivery.Id,

                Longitude = request.Longitude,
                Latitude = request.Latitude,
                TimeStamp = DateTime.UtcNow
            };
            await _deliveryLocationRepository.AddAsync(deliveryLocation);
            var ret = new ResponseDeliveryLocationDto
            {
                Id = deliveryLocation.Id,
                DeliveryId = deliveryLocation.DeliveryId,
                Latitude = deliveryLocation.Latitude,
                Longitude = deliveryLocation.Longitude,
                TimeStamp = deliveryLocation.TimeStamp

            };
            return Result<ResponseDeliveryLocationDto>.Success(ret);



        }

        public async Task<Result<List<ResponseGetDeliveryLocationDto>>> GetAsync(string userId, int deliveryId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Result<List<ResponseGetDeliveryLocationDto>>.NotFound("User not found");
            }
            var isSuperAdmin = await _userManager.IsInRoleAsync(user, "SuperAdmin");


            var delivery = await _deliveryRepository.GetByIdAsync(deliveryId);
            if (delivery is null)
            {
                return Result<List<ResponseGetDeliveryLocationDto>>.NotFound("Delivery not found");
            }
            var order = await _orderRepository.GetByIdAsync(delivery.OrderId);
            if (order is null)
            {
                return Result<List<ResponseGetDeliveryLocationDto>>.NotFound("Order not found");
            }
            if (!(order.UserId == user.Id || delivery.DriverId == userId || isSuperAdmin))
            {
                return Result<List<ResponseGetDeliveryLocationDto>>.BadRequest("This user  isn't involved in process");
            }



            var deliveryLocations = await _deliveryLocationRepository.GetAllLocationByDeliveryAsync(deliveryId);
            if (!deliveryLocations.Any())
            {
                return Result<List<ResponseGetDeliveryLocationDto>>.BadRequest("There aren't any delivery location");
            }
            var ret = deliveryLocations.Select(e => new ResponseGetDeliveryLocationDto
            {
                Id = e.Id,
                Latitude = e.Latitude,
                Longitude = e.Longitude,
                TimeStamp = e.TimeStamp
            }).ToList();
            return Result<List<ResponseGetDeliveryLocationDto>>.Success(ret);



        }
    }
}
