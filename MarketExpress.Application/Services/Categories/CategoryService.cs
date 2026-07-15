using MarketExpress.Application.Dto.Category.Request;
using MarketExpress.Application.Dto.Category.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.EntityFrameworkCore;

namespace MarketExpress.Application.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<ResponseCategory>> AddAsync(RequestAddCategory request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Result<ResponseCategory>.BadRequest("حقل الاسم مطلوب");

            }
            var name = request.Name.ToLower().Trim();
            if (await _repository.IsCategoryFound(name))
            {
                return Result<ResponseCategory>.BadRequest("الفئة موجودة مسبقا");
            }
            var cat = new Category
            {
                Name =name
            };

            await _repository.AddAsync(cat);
            var newCategory = new ResponseCategory
            {
                Id = cat.Id,
                Name = cat.Name,
            };
            return Result<ResponseCategory>.Success(newCategory);
        }

        public async Task<Result<List<ResponseCategory>>> GetAllCategoryAsync()
        {
            var categories = await _repository.GetTableNoTracking().ToListAsync();
            var ret= new List<ResponseCategory>();
            if (categories.Count == 0)
            {
                return Result<List<ResponseCategory>>.Success(ret);
            }
            ret = categories.Select(e => new ResponseCategory
            {
                Id = e.Id,
                Name = e.Name,
            }).ToList();
            return Result<List<ResponseCategory>>.Success(ret);
        }

        public async Task<Result> SoftDeleteAsync(int categoryId)
        {
            var category = await _repository.GetByIdAsync(categoryId);
            if (category == null)
            {
                return Result.NotFound("الفئة غير موجودة");
            }
            category.IsDeleted = true;
            category.DeletedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(category);
            return Result.Success();

        }


    }
}
