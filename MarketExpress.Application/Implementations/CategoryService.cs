using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Category.Request;
using MarketExpress.Application.Dto.Category.Response;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<CategoryResponse>> AddAsync(AddCategoryRequest request)
        {
            if (request.Name == null)
            {
                return Result<CategoryResponse>.BadRequest("Field is required");

            }
            if (await _repository.IsCategoryFound(request.Name))
            {
                return Result<CategoryResponse>.BadRequest("Category Exist ALready");
            }
            var cat = new Category
            {
                Name = request.Name
            };

            await _repository.AddAsync(cat);
            var newCategory = new CategoryResponse
            {
                Id = cat.Id,
                Name = cat.Name,
            };
            return Result<CategoryResponse>.Success(newCategory);
        }

        public async Task<Result> SoftDeleteAsync(DeleteCategoryRequest request)
        {
            var category= await _repository.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                return Result.NotFound("Category not found");
            }
            category.IsDeleted = true;
            category.DeletedAt= DateTime.UtcNow;

            await _repository.UpdateAsync(category);
            return Result.Success();

        }

       
    }
}
