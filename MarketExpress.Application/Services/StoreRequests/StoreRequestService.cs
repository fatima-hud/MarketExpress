using MarketExpress.Application.Dto.RequestStore.Request;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Application.Dto.Store.Response;
using MarketExpress.Application.Services.Emails;
using MarketExpress.Application.Services.Images;
using MarketExpress.Application.Services.OtpCodes;
using MarketExpress.Application.Services.Users;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Helping;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
namespace MarketExpress.Application.Services.StoreRequests
{
    public class StoreRequestService : IStoreRequestService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IStoreRepository _storeRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IImageService _imageService;
        private readonly IOtpCodeService _otpCodeService;

        public StoreRequestService(IUserRepository userRepository, UserManager<User> userManager, IUserService userService, IEmailService emailService, IStoreRepository storeRepository, IImageService imageService, IDocumentRepository userDocumentRepository, IOtpCodeService otpCodeService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _userService = userService;
            _emailService = emailService;
            _storeRepository = storeRepository;
            _imageService = imageService;
            _documentRepository = userDocumentRepository;
            _otpCodeService = otpCodeService;
        }
        public async Task<Result<ResponseRequestStoreDto>> AddAsync(string userId, RequestStoreDto request)
        {
            if (string.IsNullOrEmpty(request.StoreName))
            {
                return Result<ResponseRequestStoreDto>.BadRequest("اسم متجر مطلوب");
            }
            if (string.IsNullOrEmpty(request.Address))
            {
                return Result<ResponseRequestStoreDto>.BadRequest("عنوان متجر مطلوب");
            }
            if (string.IsNullOrEmpty(request.Email))
            {
                return Result<ResponseRequestStoreDto>.BadRequest("ايميل متجر مطلوب");
            }
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                return Result<ResponseRequestStoreDto>.BadRequest("رقم هاتف المتجر مطلوب ");
            }

            using var trans = await _storeRepository.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.NotFound("مستخدم غير موجود");
                }
                if (user.IsBanned)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("مستخدم محظور");
                }
                if (!user.EmailConfirmed)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("الايميل غير مؤكد");
                }
                var isUserHasStore = await _storeRepository.IsUserExistByIdAndStatueApprovedAsync(userId);
                if (isUserHasStore)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("المستخدم يملك متجر مسبقا");
                }
                var isUserHasPending = await _storeRepository.IsUserHasPendingStoreAsync(userId);
                if (isUserHasPending)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("المستخدم لديه طلب مسبقا");
                }
                if (user.BirthDate < BirthDateSettings.minDate || user.BirthDate > BirthDateSettings.maxDate)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("تاريخ الميلاد غير صحيح");
                }
                var age = DateTime.Today.Year - user.BirthDate.Year;
                if (user.BirthDate.Date > DateTime.Today.AddYears(-age)) age--;
                if (age < 18)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("عمر المستخدم يجب ان يكون على الاقل 18 سنة");
                }

                var name = request.StoreName.Trim().ToLower();
                var StoreName = await _storeRepository.IsNameExistAsync(name);
                if (StoreName)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("اسم المتجر مستخدم مسبقا");
                }
                if (request.Latitude < -90 || request.Latitude > 90)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("خطوط العرض غير صحيحة");
                }
                if (request.Longitude < -180 || request.Longitude > 180)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("خطوط الطول غير صحيحة");
                }
                if (request.WorkingHoursEnd <= request.WorkingHoursStart)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("انتهاء ساعات العمل يجب ان يكون اكبر من بداية ساعات العمل");
                }
                var duration = request.WorkingHoursEnd - request.WorkingHoursStart;
                if (duration.TotalHours > 24)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("مجال ساعات العمل غير صحيح");
                }
                if (request.WorkingHoursStart >= TimeSpan.FromHours(24) || request.WorkingHoursEnd >= TimeSpan.FromHours(24))
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("ساعات العمل غير صحيحة");
                }
                if (request.WorkingHoursStart < TimeSpan.Zero || request.WorkingHoursEnd < TimeSpan.Zero)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("ساعات العمل غير صحيحة");
                }

                var phoneNumber = request.PhoneNumber.Trim();
                var isPhoneNumberValid = await _userService.CheckFormPhoneNumberAsync(phoneNumber);
                if (!isPhoneNumberValid)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("رقم الهاتف غير صحيح");
                }
                var isPhoneNumberExistByStore = await _storeRepository.IsPhoneNumberExistAsync(phoneNumber);
                if (isPhoneNumberExistByStore)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("رقم الهاتف موجود مسبقا");
                }
                var isPhoneNumberExistByUser = await _userRepository.GetUserByPhoneNumberAsync(phoneNumber);
                if (isPhoneNumberExistByUser is not null && isPhoneNumberExistByUser.Id != userId)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("رقم الهاتف موجود مسبقا");
                }


                var email = request.Email.Trim().ToLower();
                var existUser = await _userManager.FindByEmailAsync(email);

                if (existUser != null && existUser.Id != userId)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("الايميل موجود مسبقا");
                }
                var FormEmail = await _otpCodeService.CheckFormEmailAsync(email);
                if (!FormEmail)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("@:البريد الإلكتروني يجب أن يتضمن اسم نطاق صالح ورمز");
                }
                var isEmailExistByStore = await _storeRepository.IsEmailExistAsync(email);
                if (isEmailExistByStore)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("الايميل موجود مسبقا");
                }
                 var store = new Store
                {
                    OwnerId = user.Id,
                    StoreName = request.StoreName.Trim(),
                    Description = request.Description.Trim(),
                    Address = request.Address.Trim(),
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
                    WorkingHoursStart = request.WorkingHoursStart,
                    WorkingHoursEnd = request.WorkingHoursEnd,
                    StoreStatus = enStoreStatus.Pending,
                    Email = request.Email.Trim(),
                    PhoneNumber = request.PhoneNumber.Trim(),
                };

                var FeaturedImage = await _imageService.UploadImageAsync(request.FeaturedImage);
                if (FeaturedImage is null)
                {
                   
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("فشل في تحميل صورة الغلاف");
                }
                store.FeaturedImage = FeaturedImage.Value;

                var Logo = await _imageService.UploadImageAsync(request.Logo);
                if (Logo is null)
                {
                    await _imageService.DeleteImageAsync(FeaturedImage.Value);
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("فشل في تحميل صورة شعار المتجر");
                }
                store.Logo = Logo.Value;

                await _storeRepository.AddAsync(store);
                var NationalIdFrontImage = await _imageService.UploadImageAsync(request.NationalIdFrontImage);
                if (NationalIdFrontImage is null)
                {
                    await _imageService.DeleteImageAsync(FeaturedImage.Value);
                    await _imageService.DeleteImageAsync(Logo.Value);
                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("فشل في تحميل صورة الهوية الامامية");
                }
                var documentNationalIdFront = new Document
                {
                    StoreId = store.Id,
                    OwnerType = enDocumentOwnerType.Store,
                    FilePath = NationalIdFrontImage.Value,
                    FileType = enFileDocumentType.NationalIdFrontImage,

                };

                var NationalIdBackImage = await _imageService.UploadImageAsync(request.NationalIdBackImage);
                if (NationalIdBackImage is null)
                {
                    await _imageService.DeleteImageAsync(FeaturedImage.Value);
                    await _imageService.DeleteImageAsync(Logo.Value);
                    await _imageService.DeleteImageAsync(NationalIdFrontImage.Value);

                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("فشل في تحميل صورة الهوية الخلفية");
                }
                var documentNationalIdBack = new Document
                {
                    StoreId = store.Id,
                    OwnerType = enDocumentOwnerType.Store,
                    FilePath = NationalIdBackImage.Value,
                    FileType = enFileDocumentType.NationalIdBackImage,

                };

                var StoreLicense = await _imageService.UploadImageAsync(request.StoreLicense);
                if (StoreLicense is null)
                {
                    await _imageService.DeleteImageAsync(FeaturedImage.Value);
                    await _imageService.DeleteImageAsync(Logo.Value);
                    await _imageService.DeleteImageAsync(NationalIdFrontImage.Value);
                    await _imageService.DeleteImageAsync(NationalIdBackImage.Value);

                    await trans.RollbackAsync();
                    return Result<ResponseRequestStoreDto>.BadRequest("فشل في تحميل صورة ترخيص المتجر");

                }

               
              

               
             
                var documentStoreLicense = new Document
                {
                    StoreId = store.Id,
                    OwnerType = enDocumentOwnerType.Store,
                    FilePath = StoreLicense.Value,
                    FileType = enFileDocumentType.StoreLicense,

                };


                await _documentRepository.AddAsync(documentNationalIdFront);
                await _documentRepository.AddAsync(documentNationalIdBack);
                await _documentRepository.AddAsync(documentStoreLicense);
                await trans.CommitAsync();

                var ret = new ResponseRequestStoreDto
                {
                    RequestId = store.Id,
                    StoreName = request.StoreName,
                    status = enStoreStatus.Pending,
                    CreatedAt = store.CreatedAt,

                };

                return Result<ResponseRequestStoreDto>.Success(ret);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<ResponseRequestStoreDto>.Failure($"حدث خطا داخلي {ex.Message}");
            }
        }

        public async Task<Result<List<ResponseOwnerStoreDto>>> GetAllRequestStoreByUserAsync(string userId)
        {
            var res = await _storeRepository.GetAllRequestStoresByUserAsync(userId);
            var ret = new List<ResponseOwnerStoreDto>();
            if (res is null)
            {
                return Result<List<ResponseOwnerStoreDto>>.Success(ret);
            }
            ret = res.Select(e => new ResponseOwnerStoreDto
            {
                Id = e.Id,
                StoreName = e.StoreName,
                StoreEmail = e.Email,
                StorePhoneNumber = e.PhoneNumber,
                Address = e.Address,
                StoreStatus = e.StoreStatus,
                Description = e.Description,
                Longitude = e.Longitude,
                Latitude = e.Latitude,
                WorkingHoursStart = e.WorkingHoursStart,
                WorkingHoursEnd = e.WorkingHoursEnd,
                CreatedAt = e.CreatedAt,
                ApprovedAt = e.ApprovedAt,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                FeaturedImage = e.FeaturedImage,

                Logo = e.Logo,

                IsActive = e.IsActive,


                Note = e.Note,
                RejectedAt = e.RejectedAt,


            }).ToList();
            return Result<List<ResponseOwnerStoreDto>>.Success(ret);


        }

        public async Task<Result<ResponseGetFilesByStoreDto>> GetFilesByStoreAsync(string userId, int storeId)
        {
            var store = await _storeRepository.GetByIdAsync(storeId);
            if (store == null)
            {
                return Result<ResponseGetFilesByStoreDto>.NotFound("متجر غير موجود ");
            }
            var currentUser = await _userManager.FindByIdAsync(userId);
            if (currentUser is null)
            {
                return Result<ResponseGetFilesByStoreDto>.NotFound("مالك المتجر غير موجود");
            }
            var isSuperAdmin = await _userManager.IsInRoleAsync(currentUser, "SuperAdmin");
            if (userId != store.OwnerId && !isSuperAdmin)
            {
                return Result<ResponseGetFilesByStoreDto>.BadRequest("المستخدم غير مصرح له رؤية الملفات");
            }
            var frontImage = await _documentRepository.GetStoreDocumentByNationalIdFrontImageAsync(storeId);
            if (frontImage is null)
            {
                return Result<ResponseGetFilesByStoreDto>.NotFound("صورة الهوية الامامية غير موجودة");
            }
            var backImage = await _documentRepository.GetStoreDocumentByNationalIdBackImageAsync(storeId);
            if (backImage is null)
            {
                return Result<ResponseGetFilesByStoreDto>.NotFound("صورة الهوية الخلفية غير موجودة");
            }
            var license = await _documentRepository.GetStoreDocumentByStoreLicenseImageAsync(storeId);
            if (license is null)
            {
                return Result<ResponseGetFilesByStoreDto>.NotFound("صورة ترخيص المتجر غير موجودة");
            }
            var ret = new ResponseGetFilesByStoreDto
            {
                NationalIdFrontImage = frontImage.FilePath,
                NationalIdBackImage = backImage.FilePath,
                StoreLicenseImage = license.FilePath

            };
            return Result<ResponseGetFilesByStoreDto>.Success(ret);
        }

        public async Task<Result<List<ResponseOwnerStoreDto>>> GetFilterRequestStoresByUserAsync(string userId, enStoreStatus storeStatus)
        {
            var res = await _storeRepository.GetFilterRequestStoresByUserAsync(userId, storeStatus);
            var ret = new List<ResponseOwnerStoreDto>();
            if (res is null)
            {
                return Result<List<ResponseOwnerStoreDto>>.Success(ret);
            }
            ret = res.Select(e => new ResponseOwnerStoreDto
            {
                Id = e.Id,
                StoreName = e.StoreName,
                StoreEmail = e.Email,
                StorePhoneNumber = e.PhoneNumber,
                Address = e.Address,
                StoreStatus = e.StoreStatus,
                Description = e.Description,
                Longitude = e.Longitude,
                Latitude = e.Latitude,
                WorkingHoursStart = e.WorkingHoursStart,
                WorkingHoursEnd = e.WorkingHoursEnd,
                CreatedAt = e.CreatedAt,
                ApprovedAt = e.ApprovedAt,
                IsDeleted = e.IsDeleted,
                DeletedAt = e.DeletedAt,
                FeaturedImage = e.FeaturedImage,

                Logo = e.Logo,

                IsActive = e.IsActive,


                Note = e.Note,
                RejectedAt = e.RejectedAt,


            }).ToList();
            return Result<List<ResponseOwnerStoreDto>>.Success(ret);

        }
    }
}
