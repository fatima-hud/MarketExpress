using MarketExpress.Application.Dto.Notification.Request;
using MarketExpress.Application.Dto.Notification.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace MarketExpress.Application.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IStoreRepository _storeRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserNotificationRepository _userNotificationRepository;

        public NotificationService(UserManager<User> userManager, IStoreRepository storeRepository, INotificationRepository notificationRepository, IUserNotificationRepository userNotificationRepository)
        {
            _userManager = userManager;
            _storeRepository = storeRepository;
            _notificationRepository = notificationRepository;
            _userNotificationRepository = userNotificationRepository;

        }
        public async Task<Result<ResponseAddNotificationDto>> AddAsync(string ownerStoreId, RequestAddNotificationDto request)
        {
            var trans = await _notificationRepository.BeginTransactionAsync();
            try
            {
                var owner = await _userManager.FindByIdAsync(ownerStoreId);
                if (owner is null)
                {
                    return Result<ResponseAddNotificationDto>.NotFound("صاحب متجر غير موجود");
                }
                var userRoles = await _userManager.GetRolesAsync(owner);
                if (!userRoles.Contains("Admin"))
                {
                    return Result<ResponseAddNotificationDto>.BadRequest("مستخدم لا يملك متجر");
                }
                var store = await _storeRepository.GetStoreByAdminAsync(ownerStoreId);
                if (store is null)
                {
                    return Result<ResponseAddNotificationDto>.BadRequest("مستخدم لا يملك متجر");
                }

                var isDuplicate = await _notificationRepository.DuplicateCheckAsync(ownerStoreId, request.Title, request.Message, request.Type);
                if (isDuplicate)
                {
                    return Result<ResponseAddNotificationDto>.BadRequest("تم اضافة الاشعار مسبقا");

                }

                List<string> users;
                if (!request.IsGlobal)
                {
                    if (request.UserIds == null || !request.UserIds.Any())
                        return Result<ResponseAddNotificationDto>.BadRequest("يجب تحديد المستخدمين اذا الاشعار غير عام");


                    var userIds = request.UserIds
                        .Where(id => !string.IsNullOrWhiteSpace(id))
                        .Distinct()
                        .ToList();

                    if (!userIds.Any())
                        return Result<ResponseAddNotificationDto>.BadRequest("قيم معرفات المستخدمين غير صحيحة");

                    users = await _userManager.Users
                        .Where(u => userIds.Contains(u.Id))
                        .Select(u => u.Id)
                        .ToListAsync();

                    var notFoundUsers = userIds.Except(users).ToList();
                    if (notFoundUsers.Any())
                        return Result<ResponseAddNotificationDto>.NotFound($"المستخدمين غير موجودين: {string.Join(",", notFoundUsers)}");
                }
                else
                {
                    users = await _userManager.Users
                        .Where(e => !e.IsBanned)
                        .Select(e => e.Id)
                        .ToListAsync();
                }


                if (!users.Any())
                {
                    return Result<ResponseAddNotificationDto>
                        .BadRequest("لا يوجد مستخدمين متاحين لارسال الاشعار");
                }


                var notification = new Notification
                {
                    CreatedById = owner.Id,
                    Message = request.Message,
                    CreatedAt = DateTime.UtcNow,
                    Title = request.Title,
                    Type = request.Type,
                    IsGlobal = request.IsGlobal,
                    StoreId = store.Id,

                };
                await _notificationRepository.AddAsync(notification);


                var sendUsers = users.Select(e => new UserNotification
                {
                    NotificationId = notification.Id,
                    UserId = e,
                    IsRead = false
                }).ToList();
                await _userNotificationRepository.AddRangeAsync(sendUsers);
                var response = new ResponseAddNotificationDto
                {
                    Id = notification.Id,
                    CreatedById = notification.CreatedById,
                    StoreId = store.Id,
                    StoreName = store.StoreName,
                    Message = notification.Message,
                    Title = notification.Title,
                    Type = notification.Type,
                    CreatedAt = notification.CreatedAt,

                    IsGlobal = notification.IsGlobal,
                    UserIds = users
                };
                await trans.CommitAsync();
                return Result<ResponseAddNotificationDto>.Success(response);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<ResponseAddNotificationDto>.Failure($"حدث خطا داخلي {ex.Message}");
            }
        }

        public async Task<Result<List<ResponseGetNotificationDto>>> GetAllNotificationAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Result<List<ResponseGetNotificationDto>>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<List<ResponseGetNotificationDto>>.BadRequest("مستخدم محظور");
            }
            var notifications = await _userNotificationRepository.GetAllNotificationByUserAsync(userId);
            var ret = notifications.Select(e => new ResponseGetNotificationDto
            {
                NotificationId = e.NotificationId,
                CreatedBy = e.Notification.CreatedById,
                CreatedAt = e.Notification.CreatedAt,
                IsGlobal = e.Notification.IsGlobal,
                IsRead = e.IsRead,
                ReadAt = e.ReadAt,
                Message = e.Notification.Message,
                Type = e.Notification.Type,
                Title = e.Notification.Title,
                StoreId = e.Notification.StoreId,
                StoreName = e.Notification.Store.StoreName,

            }).ToList();
            return Result<List<ResponseGetNotificationDto>>.Success(ret);

        }

        public async Task<Result<List<ResponseGetNotificationDto>>> GetAllUnReadNotificationAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Result<List<ResponseGetNotificationDto>>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {
                return Result<List<ResponseGetNotificationDto>>.BadRequest("مستخدم محظور");
            }
            var notifications = await _userNotificationRepository.GetAllUnReadNotinficationByUserAsync(userId);
            var ret = notifications.Select(e => new ResponseGetNotificationDto
            {
                NotificationId = e.NotificationId,
                CreatedBy = e.Notification.CreatedById,
                CreatedAt = e.Notification.CreatedAt,
                IsGlobal = e.Notification.IsGlobal,
                IsRead = e.IsRead,
                ReadAt = e.ReadAt,
                Message = e.Notification.Message,
                Type = e.Notification.Type,
                Title = e.Notification.Title,
                StoreId = e.Notification.StoreId,
                StoreName = e.Notification.Store.StoreName,

            }).ToList();
            return Result<List<ResponseGetNotificationDto>>.Success(ret);
        }

        public async Task<Result<ResponseReadNotificationDto>> MarkAsReadAsync(string userId, int notificationId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                return Result<ResponseReadNotificationDto>.NotFound("مستخدم غير موجود");
            }
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification is null)
            {
                return Result<ResponseReadNotificationDto>.NotFound("الاشعار غير وجود");
            }
            var userNotification = await _userNotificationRepository.GetNotificationByUserAsync(userId, notificationId);
            if (userNotification is null)
            {
                return Result<ResponseReadNotificationDto>.NotFound("الاشعار غير موجود لهذا المستخدم");
            }
            if (userNotification.IsRead)
            {
                return Result<ResponseReadNotificationDto>.BadRequest("الاشعار مقروء مسبقا");
            }
            userNotification.IsRead = true;
            userNotification.ReadAt = DateTime.UtcNow;
            await _userNotificationRepository.UpdateAsync(userNotification);



            var ret = new ResponseReadNotificationDto
            {
                NotificationId = notificationId,
                IsRead = userNotification.IsRead,
                ReadAt = userNotification.ReadAt
            };
            return Result<ResponseReadNotificationDto>.Success(ret);

        }
    }
}
