using MarketExpress.Application.Dto;
using MarketExpress.Application.Dto.Order.Response;
using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Application.Services.AesEncryption;
using MarketExpress.Core.DtoHelping.Roles;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Helping;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.Abstracts;
using Microsoft.AspNetCore.Identity;
namespace MarketExpress.Application.Services.SuperAdmins
{
    public class SuperAdminService : ISuperAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IStoreRepository _storeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDocumentRepository _userDocumentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IAesEncryptionService _aesEncryptionService;
        private readonly IRoleRepository _roleRepository;

        public SuperAdminService(UserManager<User> userManager, IRefreshTokenRepository refreshTokenRepository, RoleManager<IdentityRole> roleManager, IStoreRepository storeRepository, IUserRepository userRepository, IDocumentRepository userDocumentRepository, IOrderRepository orderRepository, IAesEncryptionService aesEncryptionService, IRoleRepository roleRepository)
        {
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _roleManager = roleManager;
            _storeRepository = storeRepository;
            _userRepository = userRepository;
            _userDocumentRepository = userDocumentRepository;
            _orderRepository = orderRepository;
            _aesEncryptionService = aesEncryptionService;
            _roleRepository = roleRepository;
        }
        public async Task<Result<bool>> RevokeTokenAsync(string userId)
        {
            var trans = await _refreshTokenRepository.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null || user.IsDeleted)
                {
                    return Result<bool>.NotFound("مسنخدم غير موجود");
                }
                if (user.IsBanned)
                {
                    return Result<bool>.BadRequest("مستخدم محظور");
                }

                user.IsBanned = true;
                await _refreshTokenRepository.RevokeAllRefreshTokensAsync(userId);
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    await trans.RollbackAsync();
                    return Result<bool>.Failure("فشل في حظر مستخدم");
                }
                var store = await _storeRepository.GetStoreByAdminAsync(userId);
                if (store != null)
                {
                    store.IsActive = false;
                    await _storeRepository.SaveChangesAsync();
                }

                await trans.CommitAsync();
                return Result<bool>.Success(true, "تم حظر المستخدم وإلغاء جميع رموز التحديث");
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<bool>.Failure("حدث خطا داخلي");
            }
        }
        public async Task<Result<bool>> UnbanUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || user.IsDeleted)
                return Result<bool>.NotFound("مستخدم غير موجود");

            if (!user.IsBanned)
                return Result<bool>.BadRequest("المستخدم غير محظور");

            user.IsBanned = false;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return Result<bool>.Failure("فشل في فك الحظر");

            var store = await _storeRepository.GetUnActiveStoreByAdminAsync(userId);
            if (store != null)
            {
                store.IsActive = true;
                await _storeRepository.SaveChangesAsync();
            }


            return Result<bool>.Success(true, "تم فك الحظر بنجاح");
        }
        public async Task<Result> AddRoleAsync(string SadminId, AddRoleDto dto)
        {
            var admin = await _userManager.FindByIdAsync(SadminId);

            if (admin is null || !await _userManager.IsInRoleAsync(admin, "SuperAdmin"))
            {
                return Result.BadRequest("ليس لديه صلاحيات مالك التطبيق");
            }
            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user is null || user.IsDeleted || !await _roleManager.RoleExistsAsync(dto.Role))
            {
                return Result.NotFound("المستخدم او الدور غير موجود");
            }


            if (user.IsBanned)
            {

                return Result.BadRequest("مستخدم محظور");
            }
            if (!user.EmailConfirmed)
            {

                return Result.BadRequest("الايميل غير مؤكد");
            }
            if (dto.Role != "EmployeeOfPayment")
            {
                return Result.BadRequest("غير مسموح سوى اضافة دور موظف دفع");
            }

            if (await _userManager.IsInRoleAsync(user, dto.Role))
            {
                return Result.BadRequest("المستخدم لديه الدور مسبقا");
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                return Result.BadRequest("  لا يمكن اسناد دور موظف الدفع لصاحب متجر");
            }
            if (roles.Contains("SuperAdmin"))
            {
                return Result.BadRequest("لا يمكن اسناد دور موظف الدفع لسوبر ادمن");
            }

            if (user.BirthDate < BirthDateSettings.minDate || user.BirthDate > BirthDateSettings.maxDate)
            {

                return Result.BadRequest("تاريخ الميلاد غير صحيح");
            }
            var age = DateTime.Today.Year - user.BirthDate.Year;
            if (user.BirthDate.Date > DateTime.Today.AddYears(-age)) age--;
            if (age < 18)
            {
                return Result.BadRequest("عمر المستخدم يجب ان يكون على الاقل 18 سنة");
            }
            var res = await _userManager.AddToRoleAsync(user, dto.Role);
            if (!res.Succeeded)
            {
                return Result.BadRequest("لم يتم اضافة الدور");
            }
            return Result.Success("تم اضافة الدور بنجاح");
        }
        public async Task<Result<ResponseApprovedStoreRequestDto>> ApprovedStoreRequestAsync(int requestId)
        {
            using var trans = await _storeRepository.BeginTransactionAsync();
            try
            {
                var request = await _storeRepository.GetByIdAsync(requestId);
                if (request == null)
                {
                    return Result<ResponseApprovedStoreRequestDto>.NotFound("الطلب غير موجود");
                }
                if (request.StoreStatus == enStoreStatus.Approved)
                {
                    return Result<ResponseApprovedStoreRequestDto>.BadRequest("هذا المتجر تم قبوله مسبقا ");
                }
                if (request.StoreStatus == enStoreStatus.Rejected)
                {
                    return Result<ResponseApprovedStoreRequestDto>.BadRequest("هذا المتجر تم رفضه مسبقا");
                }
                if (request.StoreStatus == enStoreStatus.Cancelled)
                {
                    return Result<ResponseApprovedStoreRequestDto>.BadRequest("هذا المتجر تم الغاؤه مسبقا ");
                }
                if (request.StoreStatus == enStoreStatus.Deleted)
                {
                    return Result<ResponseApprovedStoreRequestDto>.BadRequest("هذا المتجر تم حذفه مسبقا ");
                }
                var user = await _userManager.FindByIdAsync(request.OwnerId);
                if (user == null)
                    return Result<ResponseApprovedStoreRequestDto>.NotFound("صاحب الطلب غير موجود");
                await _userManager.AddToRoleAsync(user, "Admin");
                var ApprovedTime = DateTime.UtcNow;
                request.ApprovedAt = ApprovedTime;
                request.StoreStatus = enStoreStatus.Approved;
                request.IsActive = true;
                await _storeRepository.UpdateAsync(request);
                var ret = new ResponseApprovedStoreRequestDto
                {
                    StoreName = request.StoreName,
                    ApprovedAt = ApprovedTime,
                    RequestId = requestId,
                    status = enStoreStatus.Approved
                };
                await trans.CommitAsync();
                return Result<ResponseApprovedStoreRequestDto>.Success(ret, " طلبك تم الموافقة عليه");
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<ResponseApprovedStoreRequestDto>.Failure($"حدث خطا داخلي {ex.Message}");
            }
        }
        public async Task<Result<List<ResponsePendingStoreDto>>> GetAllPendingRequestStore()
        {
            var stores = await _storeRepository.GetAllPendingStoreAsync();
            if (stores is null)
            {
                return Result<List<ResponsePendingStoreDto>>.NotFound("لا يوجد طلبات قيد الانتظار");
            }
            var ret = new List<ResponsePendingStoreDto>();
            ret = stores.Select(e => new ResponsePendingStoreDto
            {
                Id = e.Id,
                OwnerId = e.OwnerId,
                StoreName = e.StoreName,
                StoreStatus = e.StoreStatus,
                Address = e.Address,
                PhoneNumber = e.PhoneNumber,
                Email = e.Email,
                WorkingHoursStart = e.WorkingHoursStart,
                WorkingHoursEnd = e.WorkingHoursEnd,
                CreatedAt = e.CreatedAt,
                IsActive = e.IsActive,
                Description = e.Description,
                //     Latitude = e.Latitude,
                //    Longitude = e.Longitude,
                Logo = e.Logo,
                FeaturedImage = e.FeaturedImage
            }).ToList();
            return Result<List<ResponsePendingStoreDto>>.Success(ret);
        }
        public async Task<Result<ResponseRejectedStoreRequestDto>> RejectedStoreRequestAsync(RequestRejectdDto dto)
        {
            var request = await _storeRepository.GetByIdAsync(dto.RequestId);
            if (request == null)
            {
                return Result<ResponseRejectedStoreRequestDto>.NotFound("الطلب غير موجود");
            }
            if (request.StoreStatus == enStoreStatus.Approved)
            {
                return Result<ResponseRejectedStoreRequestDto>.BadRequest("هذا المتجر تم قبوله مسبقا ");
            }
            if (request.StoreStatus == enStoreStatus.Rejected)
            {
                return Result<ResponseRejectedStoreRequestDto>.BadRequest(" هذا المتجر تم رفضه مسبقا");
            }
            if (request.StoreStatus == enStoreStatus.Cancelled)
            {
                return Result<ResponseRejectedStoreRequestDto>.BadRequest("هذا المتجر تم الغاؤه مسبقا ");
            }
            if (request.StoreStatus == enStoreStatus.Deleted)
            {
                return Result<ResponseRejectedStoreRequestDto>.BadRequest("هذا المتجر تم حذفه مسبقا ");
            }
            var rejectedTime = DateTime.UtcNow;
            request.RejectedAt = rejectedTime;
            request.StoreStatus = enStoreStatus.Rejected;
            request.IsActive = false;
            request.Note = dto.RejectionReason;
            var ret = new ResponseRejectedStoreRequestDto
            {
                RequestId = request.Id,
                StoreName = request.StoreName,
                RejectedAt = rejectedTime,
                status = enStoreStatus.Rejected,
                Note = dto.RejectionReason
            };
            await _storeRepository.UpdateAsync(request);
            return Result<ResponseRejectedStoreRequestDto>.Success(ret, " الطلب تم رفضه بنجاح");
        }

        public async Task<Result<List<ResponsePendingStoreDto>>> GetAllRequestStoreByFilter(enStoreStatus storeStatus, int pageNumber, int pageSize)
        {
            var stores = await _storeRepository.GetAllRequestStoreByFilter(storeStatus, pageNumber, pageSize);
            var ret = new List<ResponsePendingStoreDto>();
            if (stores is null)
            {
                return Result<List<ResponsePendingStoreDto>>.Success(ret);
            }

            ret = stores.Select(e => new ResponsePendingStoreDto
            {
                Id = e.Id,
                OwnerId = e.OwnerId,
                StoreName = e.StoreName,
                StoreStatus = e.StoreStatus,
                Address = e.Address,
                PhoneNumber = e.PhoneNumber,
                Email = e.Email,
                WorkingHoursStart = e.WorkingHoursStart,
                WorkingHoursEnd = e.WorkingHoursEnd,
                CreatedAt = e.CreatedAt,
                IsActive = e.IsActive,
                Description = e.Description,

                Logo = e.Logo,
                FeaturedImage = e.FeaturedImage
            }).ToList();
            return Result<List<ResponsePendingStoreDto>>.Success(ret);
        }

        public async Task<Result<List<ResponseAddOrderDto>>> GetAllFilterOrders(enOrderStatus orderStatus, int pageNumber, int pageSize)
        {
            var res = await _orderRepository.GetAllFilterOrdersAsync(orderStatus, pageNumber, pageSize);
            if (!res.Any())
            {
                return Result<List<ResponseAddOrderDto>>.Success(new List<ResponseAddOrderDto>());
            }

            var ret = res.Select(o => new ResponseAddOrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                CreatedAt = o.CreatedAt,
                FullName = o.User.FullName,
                Address = _aesEncryptionService.Decrypt(o.Address),
                PhoneNumber = o.User.PhoneNumber,
                Status = o.Status,
                TotalPrice = o.TotalPrice

            });
            return Result<List<ResponseAddOrderDto>>.Success(ret.ToList());
        }

        public async Task<Result> RemoveRoleAsync(string SadminId, AddRoleDto dto)
        {
            var admin = await _userManager.FindByIdAsync(SadminId);

            if (admin is null || !await _userManager.IsInRoleAsync(admin, "SuperAdmin"))
            {
                return Result.BadRequest("ليس لديه صلاحيات مالك التطبيق");
            }
            if (dto.Role == "SuperAdmin")
            {
                return Result.BadRequest("لا يمكن إزالة دور سوبر ادمن");
            }

            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user is null || user.IsDeleted || !await _roleManager.RoleExistsAsync(dto.Role))
            {
                return Result.NotFound("المستخدم أو الدور غير موجود");
            }



            var store = await _storeRepository.GetStoreByAdminAsync(dto.UserId);

            if (store != null && dto.Role == "Admin")
            {
                return Result.BadRequest(" المستخدم لديه متجر، لا يمكن إلغاء دوره");
            }

            if (!await _userManager.IsInRoleAsync(user, dto.Role))
            {
                return Result.BadRequest("المستخدم لا يملك هذا الدور");
            }

            var res = await _userManager.RemoveFromRoleAsync(user, dto.Role);

            if (!res.Succeeded)
            {
                return Result.BadRequest("لم يتم إزالة الدور");
            }

            return Result.Success("تم إزالة الدور بنجاح");
        }

        public async Task<Result<List<ResponseGetRolesByUserDto>>> GetAllRolesByUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null || user.IsDeleted)
            {
                return Result<List<ResponseGetRolesByUserDto>>.NotFound("المستخدم غير موجود ");
            }
            var roleNames = await _userManager.GetRolesAsync(user);

            var roles = await _roleRepository.GetRolesByUserAsync(userId);

            return Result<List<ResponseGetRolesByUserDto>>.Success(roles);


        }
    }
}
