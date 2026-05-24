using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.RequestStore.Response;
using MarketExpress.Application.Dto.Store.Request;
using MarketExpress.Application.Dto.Store.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
namespace MarketExpress.Application.Implementations
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IImageService _imageService;
        private readonly UserManager<User> _userManager;
        private readonly IDocumentRepository _documentRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        public StoreService(IStoreRepository repository, IImageService imageService, UserManager<User> userManager, IRoleRepository roleRepository, IDocumentRepository documentRepository, IUserService userService, IUserRepository userRepository, IProductRepository productRepository)
        {
            _storeRepository = repository;
            _imageService = imageService;
            _userManager = userManager;
            _roleRepository = roleRepository;
            _documentRepository = documentRepository;
            _userService = userService;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }
        public async Task<Result> SoftDeleteAsync(string adminId, int storeId)
        {
            var trans = await _storeRepository.BeginTransactionAsync();
            try
            {
                var store = await _storeRepository.GetByIdAsync(storeId);
                if (store == null)
                {
                    await trans.RollbackAsync();
                    return Result.NotFound("متجر غير موجود");
                }
                if (store.IsDeleted)
                {
                    await trans.RollbackAsync();
                    return Result.BadRequest("متجر محذوف مسبقا");
                }
                var currentUser = await _userManager.FindByIdAsync(adminId);
                if (currentUser is null)
                {
                    await trans.RollbackAsync();
                    return Result.NotFound("مالك المتجر غير موجود");
                }
                var isSuperAdmin = await _userManager.IsInRoleAsync(currentUser, "SuperAdmin");
                if (adminId != store.OwnerId && !isSuperAdmin)
                {
                    await trans.RollbackAsync();
                    return Result.BadRequest(" غير مصرح لك حذف المتجر");
                }
                var storeOwner = await _userManager.FindByIdAsync(store.OwnerId);
                if (storeOwner is null)
                {
                    await trans.RollbackAsync();
                    return Result.NotFound("مالك المتجر غير موجود");
                }
                if (store.StoreStatus != enStoreStatus.Approved)
                {
                    await trans.RollbackAsync();
                    return Result.NotFound($"المتجر مسبقا {store.StoreStatus} ");

                }
                var roleResult = await _userManager.RemoveFromRoleAsync(storeOwner, "Admin");
                if (!roleResult.Succeeded)
                {
                    await trans.RollbackAsync();
                    return Result.BadRequest("فشل في حذف دور الادمن");
                }
                store.IsDeleted = true;
                store.DeletedAt = DateTime.UtcNow;
                store.IsActive = false;
                store.StoreStatus = enStoreStatus.Deleted;
                await _storeRepository.UpdateAsync(store);
                await trans.CommitAsync();
                return Result.Success("تم حذف المتجر بنجاح");
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result.Failure($"حدث خطا داخلي: {ex.Message}");
            }
        }
        public async Task<Result<ResponseOwnerStoreDto>> GetStoreByAdminAsync(string ownerId)
        {
            var user = await _userManager.FindByIdAsync(ownerId);
            if (user == null)
            {
                return Result<ResponseOwnerStoreDto>.NotFound("مستخدم غير موجود");
            }
            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return Result<ResponseOwnerStoreDto>.BadRequest("مستخدم ليس ادمن");
            }
            var store = await _storeRepository.GetStoreByAdminAsync(ownerId);
            if (store is null)
            {
                return Result<ResponseOwnerStoreDto>.NotFound("متجر غير موجود");
            }
            var ret = new ResponseOwnerStoreDto
            {
                Id = store.Id,
                StoreName = store.StoreName,
                StoreEmail = store.Email,
                StorePhoneNumber = store.PhoneNumber,
                Address = store.Address,
                Description = store.Description,
                IsActive = store.IsActive,
                Latitude = store.Latitude,
                Longitude = store.Longitude,
                Logo = store.Logo,
                FeaturedImage = store.FeaturedImage,
                WorkingHoursEnd = store.WorkingHoursEnd,
                WorkingHoursStart = store.WorkingHoursStart,
                StoreStatus = store.StoreStatus,
                CreatedAt = store.CreatedAt,
                ApprovedAt = store.ApprovedAt,
                RejectedAt = store.RejectedAt,
                IsDeleted = store.IsDeleted,
                DeletedAt = store.DeletedAt,
                Note = store.Note,
            };
            return Result<ResponseOwnerStoreDto>.Success(ret);
        }
        public async Task<Result<List<ResponseStoreDto>>> GetAllStoresAsync()
        {
            var stores = await _storeRepository.GetAllActiveStoreAsync();
            var ret = new List<ResponseStoreDto>();
            if (stores is null || !stores.Any())
            {
                return Result<List<ResponseStoreDto>>.Success(ret);
            }

             ret = stores.Select(e => new ResponseStoreDto
            {
                Id = e.Id,
                StoreName = e.StoreName,
                StorePhoneNumber = e.PhoneNumber,
                Address = e.Address,
                Description = e.Description,
                StoreEmail = e.Email,
                Logo = e.Logo,
                FeaturedImage = e.FeaturedImage,
                WorkingHoursStart = e.WorkingHoursStart,
                WorkingHoursEnd = e.WorkingHoursEnd,
                Latitude = e.Latitude,
                Longitude = e.Longitude,
                IsActive = e.IsActive
            }).ToList();
            return Result<List<ResponseStoreDto>>.Success(ret);
        }

        public async Task<Result<ResponseStoreDto>> UpdateAsync(string userId, RequestUpdateStoreInfoDto request)
        {
            var trans = await _storeRepository.BeginTransactionAsync();
            try
            {
                var user=await _userManager.FindByIdAsync(userId);
                if (user == null) {
                    await trans.RollbackAsync();
                    return Result<ResponseStoreDto>.NotFound(" مستخدم غير موجود");
                }
                var store = await _storeRepository.GetByIdAsync(request.StoreId);
                if (store == null)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseStoreDto>.NotFound("متجر غير موجود");
                }
                if (store.IsDeleted)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseStoreDto>.BadRequest("متجر محذوف مسبقا");
                }
                if (!store.IsActive)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseStoreDto>.BadRequest("متجر غير نشط");
                }
                if (store.StoreStatus != enStoreStatus.Approved)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseStoreDto>.BadRequest($"متجر مسبقا{store.StoreStatus}");

                }
                if (userId != store.OwnerId)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseStoreDto>.BadRequest("غير مصرح لك تعديل المتجر");
                }
                if (!string.IsNullOrWhiteSpace(request.StoreName))
                {
                    var isStoreNameExist = await _storeRepository.IsNameExistAsync(request.StoreName, request.StoreId);
                    if (isStoreNameExist)
                    {
                        await trans.RollbackAsync();
                        return Result<ResponseStoreDto>.BadRequest("اسم المتجر مستخدم مسبقا");
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
                        return Result<ResponseStoreDto>.BadRequest("خطوط العرض غير صحيحة");
                    }
                    store.Latitude = request.Latitude.Value;
                }
                if (request.Longitude.HasValue)
                {
                    if (request.Longitude < -180 || request.Longitude > 180)
                    {
                        await trans.RollbackAsync();
                        return Result<ResponseStoreDto>.BadRequest("خطوط الطول غير صحيحة");
                    }
                    store.Longitude = request.Longitude.Value;
                }
                var start = request.WorkingHoursStart ?? store.WorkingHoursStart;
                var end = request.WorkingHoursEnd ?? store.WorkingHoursEnd;
                var invalidRange = start < TimeSpan.Zero || end < TimeSpan.Zero || start >= TimeSpan.FromHours(24) || end >= TimeSpan.FromHours(24);
                if (invalidRange)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseStoreDto>.BadRequest("ساعات العمل غير صحيحة");
                }
                if (end <= start)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseStoreDto>.BadRequest("انتهاء ساعات العمل يجب ان تكون اكبر من بداية ساعات العمل");
                }
                store.WorkingHoursStart = start;
                store.WorkingHoursEnd = end;
            
                if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                {
                    if (!await _userService.CheckFormPhoneNumberAsync(request.PhoneNumber))
                    {
                        await trans.RollbackAsync();
                        return Result<ResponseStoreDto>.BadRequest("رقم الهاتف غير صحيح");
                    }
                    if (await _storeRepository.IsPhoneNumberExistAsync(request.PhoneNumber))
                    {
                        await trans.RollbackAsync();
                        return Result<ResponseStoreDto>.BadRequest("رقم الهاتف موجود مسبقا");
                    }
                    var isPhoneNumberExistByUser=await _userRepository.GetUserByPhoneNumberAsync(request.PhoneNumber);
                    if (isPhoneNumberExistByUser is not null && isPhoneNumberExistByUser.Id != store.OwnerId)
                    {
                        await trans.RollbackAsync();
                        return Result<ResponseStoreDto>.BadRequest("رقم الهاتف موجود مسبقا");
                    }

                        store.PhoneNumber = request.PhoneNumber;
                }
           
            await _storeRepository.UpdateAsync(store);
                await trans.CommitAsync();
                var ret = new ResponseStoreDto
                {
                    Id = store.Id,
                    StoreName = store.StoreName,
                    StorePhoneNumber = store.PhoneNumber,
                    Address = store.Address,
                    Description = store.Description,
                    StoreEmail =store.Email ,
                    Logo = store.Logo,
                    FeaturedImage = store.FeaturedImage,
                    WorkingHoursStart = store.WorkingHoursStart,
                    WorkingHoursEnd = store.WorkingHoursEnd,
                    Latitude = store.Latitude,
                    Longitude = store.Longitude,
                    IsActive = store.IsActive

                };
                return Result<ResponseStoreDto>.Success(ret);


            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<ResponseStoreDto>.BadRequest($"حدث خطا داخلي {ex.Message}");
            }
            

            }

        public async Task<Result<List<ResponseGetProductDto>>> GetAllProductsByStore(int storeId)
        {
            var store=await _storeRepository.GetByIdAsync(storeId);
            if(store == null||store.IsDeleted)
            {
                return Result<List<ResponseGetProductDto>>.NotFound("المتجر غير موجود");
            }
            var ret=new List<ResponseGetProductDto>();
            var products=await _productRepository.GetAllProductByStoreAsync(storeId);
            if(ret is null)
            {
                return Result <List <ResponseGetProductDto>>.Success(ret);
            }
            foreach (var product in products) {  
                var rating = await _productRepository.GetRatingAsync(product.Id);
                var res = new ResponseGetProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Image = product.Image,
                    CategoryId = product.CategoryId,
                    Description = product.Description,
                    DiscountStartDate = product.DiscountStartDate,
                    DiscountEndDate = product.DiscountEndDate,
                    DiscountPercentage = product.DiscountPercentage,
                    PriceAfterDiscount = product.PriceAfterDiscount,
                    Rating = rating,
                    Price = product.Price
                };
                ret.Add(res);
            }
            return Result<List<ResponseGetProductDto>>.Success(ret);




        }
    }
}
