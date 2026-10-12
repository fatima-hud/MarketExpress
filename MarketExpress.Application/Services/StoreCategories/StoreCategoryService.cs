using MarketExpress.Application.Dto.StoreCategory.Request;
using MarketExpress.Application.Dto.StoreCategory.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Application.Services.StoreCategories
{
    public class StoreCategoryService : IStoreCategoryService
    {

        private readonly IStoreCategoryRepository _storeCategoryRepository;
        private readonly UserManager<User> _userManager;
        private readonly IStoreRepository _storeRepository;
        private readonly ICategoryRepository _categoryRepository;

        public StoreCategoryService(IStoreCategoryRepository storeCategoryRepository, UserManager<User> userManager, IStoreRepository storeRepository, ICategoryRepository categoryRepository)
        {
            _storeCategoryRepository = storeCategoryRepository;
            _userManager = userManager;
            _storeRepository = storeRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<ResponseGetStoreCategoryDto>> AddAsync(string userId, RequestAddStoreCategoryDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result<ResponseGetStoreCategoryDto>.NotFound("مستخدم غير موجود");
            }
            var store = await _storeRepository.GetStoreByAdminAsync(userId);
            if (store == null)
            {
                return Result<ResponseGetStoreCategoryDto>.NotFound("متجر غير موجود");
            }
            if (!store.IsActive)
            {
                return Result<ResponseGetStoreCategoryDto>.BadRequest("متجر غير نشط");
            }
            var isSuperAdmin = await _userManager.IsInRoleAsync(user, "SuperAdmin");

            if (store.OwnerId != userId && !isSuperAdmin)
            {
                return Result<ResponseGetStoreCategoryDto>.BadRequest("مستخدم غير مصرح");
            }
            if (request.CategoryId <= 0)
            {
                return Result<ResponseGetStoreCategoryDto>.BadRequest("بيانات خاطئة");
            }

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                return Result<ResponseGetStoreCategoryDto>.NotFound("فئة غير موجودة");
            }
            var isCategoryExist = await _storeCategoryRepository.IsCategoryExistByStoreAsync(store.Id, request.CategoryId);
            if (isCategoryExist)
            {
                return Result<ResponseGetStoreCategoryDto>.BadRequest("الفئة موجودة مسبقا في هذا المتجر");
            }
            var storeCategory = new StoreCategory
            {
                StoreId = store.Id,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.UtcNow

            };

            await _storeCategoryRepository.AddAsync(storeCategory);
            var res = new ResponseGetStoreCategoryDto
            {
                Id = storeCategory.Id,
                StoreId = storeCategory.StoreId,
                CategoryId = storeCategory.CategoryId,
                CreatedAt = storeCategory.CreatedAt
            };

            return Result<ResponseGetStoreCategoryDto>.Success(res);
        }

        public async Task<Result> DeleteAsync(string userId, int storeCategoryId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result.NotFound("مستخدم غير موجود");
            }
            var storeCategory = await _storeCategoryRepository.GetByIdAsync(storeCategoryId);
            if (storeCategory == null)
            {
                return Result.NotFound("الفئة غير موجودة في هذا المتجر");
            }
            var store = await _storeRepository.GetByIdAsync(storeCategory.StoreId);
            if (store == null)
            {
                return Result.NotFound("متجر غير موجود");
            }
            if (!store.IsActive)
            {
                return Result.BadRequest("متجر غير نشط");
            }
            var isSuperAdmin = await _userManager.IsInRoleAsync(user, "SuperAdmin");

            if (store.OwnerId != userId && !isSuperAdmin)
            {
                return Result.BadRequest("مستخدم غير مصرح");
            }


            storeCategory.IsDeleted = true;
            storeCategory.DeletedAt = DateTime.UtcNow;
            await _storeCategoryRepository.UpdateAsync(storeCategory);
            return Result.Success();
        }

        public async Task<Result<List<ResponseGetStoreCategoryDto>>> GetAllStoreCategoryAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result<List<ResponseGetStoreCategoryDto>>.NotFound("مستخدم غير موجود");
            }


            var store = await _storeRepository.GetStoreByAdminAsync(userId);
            if (store == null)
            {
                return Result<List<ResponseGetStoreCategoryDto>>.NotFound("متجر غير موجود");
            }
            if (!store.IsActive)
            {
                return Result<List<ResponseGetStoreCategoryDto>>.BadRequest("متجر غير نشط");
            }

            var storeCategories = await _storeCategoryRepository.GetAllStoreCategoryAsync(store.Id);
            var ret = new List<ResponseGetStoreCategoryDto>();
            if (storeCategories == null || storeCategories.Count == 0)
            {
                return Result<List<ResponseGetStoreCategoryDto>>.Success(ret);
            }
            ret = storeCategories.Select(sc => new ResponseGetStoreCategoryDto
            {
                Id = sc.Id,
                StoreId = sc.StoreId,
                CategoryId = sc.CategoryId,
                CreatedAt = sc.CreatedAt

            }).ToList();
            return Result<List<ResponseGetStoreCategoryDto>>.Success(ret);
        }

        public async Task<Result<List<ResponseGetStoreCategoryDto>>> GetAllStoreCategoryAsync(int storeId)
        {
            var store = await _storeRepository.GetByIdAsync(storeId);
            if (store == null)
            {
                return Result<List<ResponseGetStoreCategoryDto>>.NotFound("متجر غير موجود");
            }
            if (!store.IsActive)
            {
                return Result<List<ResponseGetStoreCategoryDto>>.BadRequest("متجر غير نشط");
            }
            var ret = new List<ResponseGetStoreCategoryDto>();

            var storeCategories = await _storeCategoryRepository.GetAllStoreCategoryAsync(store.Id);
            if (storeCategories == null || storeCategories.Count == 0)
            {
                return Result<List<ResponseGetStoreCategoryDto>>.Success(ret);
            }
            ret = storeCategories.Select(sc => new ResponseGetStoreCategoryDto
            {
                Id = sc.Id,
                StoreId = sc.StoreId,
                CategoryId = sc.CategoryId,
                CreatedAt = sc.CreatedAt

            }).ToList();
            return Result<List<ResponseGetStoreCategoryDto>>.Success(ret);
        }
    }
}
