using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Application.Dto.User;
using MarketExpress.Application.Services.Images;
using MarketExpress.Core.DtoHelping;
using MarketExpress.Core.DtoHelping.Roles;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Helping;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Text.RegularExpressions;
namespace MarketExpress.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IImageService _imageService;
        private readonly UserManager<User> _userManager;
        private readonly IStoreRepository _storeRepository;
        private readonly IUserRepository _userRepository;
       
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserService(IImageService imageService, UserManager<User> userManager, IStoreRepository storeRepository,IUserRepository userRepository,RoleManager<IdentityRole>roleManager)
        {
            _imageService = imageService;
            _userManager = userManager;
            _storeRepository = storeRepository;
            _userRepository = userRepository;
            _roleManager = roleManager;
        }
        public async Task<Result> CancelRequestAsync(string userId, int storeId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted) { 

                return Result.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result.BadRequest("مستخدم محظور");
            }
            var store = await _storeRepository.GetByIdAsync(storeId);
            if (store is null)
            {
                return Result.NotFound("الطلب غير موجود");
            }
            if (store.OwnerId != userId)
            {
                return Result.BadRequest("انت لست صاحب هذا الطلب ");
            }
            if (store.StoreStatus != enStoreStatus.Pending)
            {
                return Result.BadRequest("لا يمكن الغاء الطلب");
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
            if (user == null || user.IsDeleted)
            {
                return Result<string>.NotFound("المستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<string>.BadRequest("المستخدم محظور");
            }
           
            if (file == null)
                return Result<string>.BadRequest("لم يتم تحميل الملف");
            var image = await _imageService.UploadImageAsync(file);
            if (!image.IsSuccess)
            {
                return image;
            }
            await _imageService.DeleteImageAsync(user.ProfilePhoto);
            user.ProfilePhoto = image.Value;
            await _userManager.UpdateAsync(user);
            return Result<string>.Success(image.Value);
        }
        public async Task<Result<ResponseUpdateRequestStoreDto>> UpdateInfoRequestAsync(string userId, int storeId, RequestUpdateRequestStoreDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
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
           /*     if (request.Latitude.HasValue)
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
                }*/
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
               //     Latitude = store.Latitude,
                //    Longitude = store.Longitude,
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

        public async Task<Result<ResponseUserDto>> GetUserAsync(string userId)
        {
            var user=await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result<ResponseUserDto>.NotFound("المستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<ResponseUserDto>.BadRequest("المستخدم محظور");
            }
            var ret = new ResponseUserDto
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                BirthDate = user.BirthDate,
                Email = user.Email,
                Gender = user.Gender,
                PhoneNumber = user.PhoneNumber,
                ProfilePhoto = user.ProfilePhoto

            };
            return Result<ResponseUserDto>.Success(ret);
        }

        public async Task<Result<List<ResponseGetActiveUserDto>>> GetActiveUsersAsync()
        {
            var users = await _userRepository.GetActiveUsersAsync();
            if (!users.Any())
            {
                return Result<List<ResponseGetActiveUserDto>>.Success(new List<ResponseGetActiveUserDto>());
            }
           
            

          
        

            return Result<List<ResponseGetActiveUserDto>>.Success(users);

        }

        public async Task<Result<List<ResponseUserDto>>> GetBannedUsersAsync()
        {
            var res = await _userRepository.GetBannedUsersAsync();
            if (!res.Any())
            {
                return Result<List<ResponseUserDto>>.Success(new List<ResponseUserDto>());
            }
            var ret = res.Select(u => new ResponseUserDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                BirthDate = u.BirthDate,
                Email = u.Email,
                Gender = u.Gender,
                PhoneNumber = u.PhoneNumber,
                ProfilePhoto = u.ProfilePhoto
            }).ToList();
            return Result<List<ResponseUserDto>>.Success(ret);
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result.NotFound("المستخدم غير موجود");
            }
            
            user.IsDeleted = true;

            user.IsBanned = false;
            user.UserName = $"{user.UserName}_Deleted_{Guid.NewGuid()}";
            user.Email = $"{Guid.NewGuid()}_{user.Email}";
            var store = await _storeRepository.GetStoreByAdminAsync(userId);
            if (store != null)
            {
                store.IsActive = false;
                await _storeRepository.SaveChangesAsync();
            }
            await _userManager.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<ResponseUserDto>> UpdateUserAsync(string userId, RequestUpdateUserDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result<ResponseUserDto>.NotFound("المستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<ResponseUserDto>.BadRequest("User is banned");
            }
            if (!string.IsNullOrEmpty(request.FirstName))
            {
                user.FirstName=request.FirstName;
            }
            if (!string.IsNullOrEmpty(request.LastName))
            {
                user.LastName = request.LastName;
            }
            if (!string.IsNullOrEmpty(request.PhoneNumber))
            {
                var exitPhoneNumber = await _userRepository.PhoneNumberHasFoundAsync(request.PhoneNumber);
                if (exitPhoneNumber)
                {
                    return Result<ResponseUserDto>.BadRequest("رقم الهاتف موجود مسبقا");
                }

                if (!await CheckFormPhoneNumberAsync(request.PhoneNumber))
                {
                    return Result<ResponseUserDto>.BadRequest("رقم الهاتف غير صحيح ");
                }
                user.PhoneNumber = request.PhoneNumber;
            }
            if (request.BirthDate.HasValue)
            {
                if (request.BirthDate < BirthDateSettings.minDate || request.BirthDate > BirthDateSettings.maxDate)
                {
                    return Result<ResponseUserDto>.BadRequest("تاريخ الميلاد غير صحيح");
                }
                user.BirthDate = request.BirthDate.Value;
            }
            if (request.Gender.HasValue)
            {
                if (!Enum.IsDefined(typeof(enGender), request.Gender))
                {
                    return Result<ResponseUserDto>.BadRequest("جنس المستخدم يجب ان يكون 0 للذكر و 1 للانثى");
                }
                user.Gender = request.Gender.Value;
            }
            await _userRepository.SaveChangesAsync();
            var ret = new ResponseUserDto
            {
                Id = user.Id,
                Gender = user.Gender,
                BirthDate = user.BirthDate,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfilePhoto = user.ProfilePhoto,
                UserName = user.UserName

            };
            return Result<ResponseUserDto>.Success(ret);


        }
    }
}
