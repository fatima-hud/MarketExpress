using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
namespace MarketExpress.Application.Implementations
{
    public class UserService : IUserService
    {
        private readonly IImageService _imageService;
        private readonly UserManager<User> _userManager;
        private readonly IStoreRepository _storeRepository;
        public UserService(IImageService imageService, UserManager<User> userManager, IStoreRepository storeRepository)
        {
            _imageService = imageService;
            _userManager = userManager;
            _storeRepository = storeRepository;
        }
        public async Task<Result> CancelRequestAsync(string userId, int storeId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.NotFound("User not found");
            }
            if (user.IsBanned)
            {
                return Result.BadRequest("User is banned");
            }
            var store = await _storeRepository.GetByIdAsync(storeId);
            if (store is null)
            {
                return Result.NotFound("Store not found");
            }
            if (store.OwnerId != userId)
            {
                return Result.BadRequest("You are not owner this store ");
            }
            if (store.StoreStatus != enStoreStatus.Pending)
            {
                return Result.BadRequest("You can't cancel this request");
            }
            store.StoreStatus = enStoreStatus.Cancelled;
            store.IsActive = false;
            await _storeRepository.UpdateAsync(store);
            return Result.Success();
        }
        public async Task<bool> CheckFormPhoneNumberAsync(string phoneNumber)
        {
            string pattern = @"^09[1-9]{1}[0-9]{7}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(phoneNumber);
        }
        public async Task<Result<string>> UpdateProfilePhoto(string userId, IFormFile file)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<string>.NotFound("User not found");
            }
            if (user.IsBanned)
            {
                return Result<string>.BadRequest("User is banned");
            }
            if (file == null)
                return Result<string>.BadRequest("No file provided");
            var image = await _imageService.UploadImageAsync(file);
            if (!image.IsSuccess)
            {
                return image;
            }
            user.ProfilePhoto = image.Value;
            await _userManager.UpdateAsync(user);
            return Result<string>.Success(image.Value);
        }
        public async Task<Result<ResponseUpdateRequestStoreDto>> UpdateInfoRequestAsync(string userId, int storeId, RequestUpdateRequestStoreDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<ResponseUpdateRequestStoreDto>.NotFound("User not found");
            }
            if (user.IsBanned)
            {
                return Result<ResponseUpdateRequestStoreDto>.BadRequest("User is banned");
            }
            var store = await _storeRepository.GetByIdAsync(storeId);
            if (store is null)
            {
                return Result<ResponseUpdateRequestStoreDto>.NotFound("Store not found");
            }
            if (store.OwnerId != userId)
            {
                return Result<ResponseUpdateRequestStoreDto>.BadRequest("You are not owner this store ");
            }
            if (store.StoreStatus != enStoreStatus.Pending)
            {
                return Result<ResponseUpdateRequestStoreDto>.BadRequest("You can't update this request");
            }
            var trans = await _storeRepository.BeginTransactionAsync();
            try
            {
                if (!string.IsNullOrWhiteSpace(request.StoreName))
                {
                    var isStoreNameExist = await _storeRepository.IsNameExistAsync(request.StoreName, storeId);
                    if (isStoreNameExist)
                    {
                        await trans.RollbackAsync();
                        return Result<ResponseUpdateRequestStoreDto>.BadRequest("This store name is already exist");
                    }
                    store.StoreName = request.StoreName;
                }
                if (!string.IsNullOrWhiteSpace(request.Description))
                {
                    store.Description = request.Description;
                }
                if (!string.IsNullOrWhiteSpace(request.Address))
                {
                    store.Address = request.Address;
                }
                if (request.Latitude.HasValue)
                {
                    if (request.Latitude < -90 || request.Latitude > 90)
                    {
                        await trans.RollbackAsync();
                        return Result<ResponseUpdateRequestStoreDto>.BadRequest("Invalid latitude");
                    }
                    store.Latitude = request.Latitude.Value;
                }
                if (request.Longitude.HasValue)
                {
                    if (request.Longitude < -180 || request.Longitude > 180)
                    {
                        await trans.RollbackAsync();
                        return Result<ResponseUpdateRequestStoreDto>.BadRequest("Invalid longitude");
                    }
                    store.Longitude = request.Longitude.Value;
                }
                var start = request.WorkingHoursStart ?? store.WorkingHoursStart;
                var end = request.WorkingHoursEnd ?? store.WorkingHoursEnd;
                var invalidRange = start < TimeSpan.Zero || end < TimeSpan.Zero || start >= TimeSpan.FromHours(24) || end >= TimeSpan.FromHours(24);
                if (invalidRange)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseUpdateRequestStoreDto>.BadRequest("Invalid working hours");
                }
                if (end <= start)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseUpdateRequestStoreDto>.BadRequest("Invalid Working hours");
                }
                store.WorkingHoursStart = start;
                store.WorkingHoursEnd = end;
                await _storeRepository.UpdateAsync(store);
                await trans.CommitAsync();
                var ret = new ResponseUpdateRequestStoreDto
                {
                    StoreId = store.Id,
                    StoreName = store.StoreName,
                    Description = store.Description,
                    Address = store.Address,
                    Latitude = store.Latitude,
                    Longitude = store.Longitude,
                    WorkingHoursStart = store.WorkingHoursStart,
                    WorkingHoursEnd = store.WorkingHoursEnd
                };
                return Result<ResponseUpdateRequestStoreDto>.Success(ret);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<ResponseUpdateRequestStoreDto>.BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
