using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Notification.Request;
using MarketExpress.Application.Dto.Notification.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace MarketExpress.Application.Implementations
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
        public async Task<Result<ResponseAddNotificationDto>> AddAsync(string OwnerStoreId, RequestAddNotificationDto request)
        {
            var trans = await _notificationRepository.BeginTransactionAsync();
            try
            {
                var owner= await _userManager.FindByIdAsync(OwnerStoreId);
                if (owner is null)
                {
                    return Result<ResponseAddNotificationDto>.NotFound("Owner store not found");
                }
                var userRoles = await _userManager.GetRolesAsync(owner);
                if (!userRoles.Contains("Admin"))
                {
                    return Result<ResponseAddNotificationDto>.BadRequest("User hasn't store");
                }
                var store = await _storeRepository.GetStoreByAdminAsync(OwnerStoreId);
                if (store is null)
                {
                    return Result<ResponseAddNotificationDto>.BadRequest("User hasn't store");
                }
                if (!request.IsGlobal && (request.UserIds == null || !request.UserIds.Any()))

                {
                    return Result<ResponseAddNotificationDto>.BadRequest("UserIds required when IsGlobal is false");

                }
                List<string> users;
                if (!request.IsGlobal)
                {
                    if (request.UserIds == null || !request.UserIds.Any())
                        return Result<ResponseAddNotificationDto>.BadRequest("UserIds required when IsGlobal is false");

                    // تنظيف القيم: إزالة الفراغات وتحويلها إلى قائمة فريدة
                    var userIds = request.UserIds
                        .Where(id => !string.IsNullOrWhiteSpace(id))
                        .Distinct()
                        .ToList();

                    if (!userIds.Any())
                        return Result<ResponseAddNotificationDto>.BadRequest("UserIds cannot be empty or whitespace");

                    users = await _userManager.Users
                        .Where(u => userIds.Contains(u.Id))
                        .Select(u => u.Id)
                        .ToListAsync();

                    var notFoundUsers = userIds.Except(users).ToList();
                    if (notFoundUsers.Any())
                        return Result<ResponseAddNotificationDto>.NotFound($"Users not found: {string.Join(",", notFoundUsers)}");
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
                        .BadRequest("No users available to send notification");
                }


                var notification = new Notification
                {
                    CreatedBy = owner.Id,
                    Message = request.Message,
                    CreatedAt = DateTime.UtcNow,
                    Title = request.Title,
                    Type = request.Type,
                    IsGlobal = request.IsGlobal,
                };
                await _notificationRepository.AddAsync(notification);
               
                
                var sendUsers = users.Select(e => new UserNotification
                {
                    NotificationId = notification.Id,
                    UserId = e,
                    IsRead=false
                }).ToList();
                await _userNotificationRepository.AddRangeAsync(sendUsers);
                var response = new ResponseAddNotificationDto
                {
                    Id = notification.Id,
                    CreatedBy = notification.CreatedBy,
                    StoreId=store.Id,
                    StoreName=store.StoreName,
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
                return Result<ResponseAddNotificationDto>.Failure("Something was wrong");
            }
        }
    }
}
