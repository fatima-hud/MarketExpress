using MarketExpress.Application.Abstacts;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Implementations
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<User> _userManager;

        public RatingService(IRatingRepository repository, IProductRepository productRepository, UserManager<User> userManager)
        {
            _repository = repository;
            _productRepository = productRepository;
            _userManager = userManager;
        }

        public async Task<Result<RatingResponseDto>> AddRatingAsync(string userId, RatingDto dto)
        {
           var prod=await _productRepository.GetByIdAsync(dto.ProductId);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<RatingResponseDto>.NotFound("Not found user");
            }
            if (prod == null)
            {
                return Result<RatingResponseDto>.NotFound("Not found product");
            }
            var rating = new Rating
            {
                UserId = user.Id,
                ProductId = dto.ProductId,
                RatingValue = dto.RatingValue,
                Comment = dto.Comment,
                CreatedAt=DateTime.UtcNow
            };


            await _repository.AddAsync(rating);
            var res = new RatingResponseDto
            {
                UserId = user.Id,
                ProductId = dto.ProductId,
                RatingValue = dto.RatingValue.HasValue?dto.RatingValue.Value:0,
                Comment = dto.Comment,
                CreatedAt = rating.CreatedAt
            };
            return Result<RatingResponseDto>.Success(res);


        }

        public async Task<Result> DeleteRatingAsync(string userId, int id)
        {
            var rate= await _repository.GetByIdAsync(id);
           
            var user = await _userManager.FindByIdAsync(userId);
            if (id == null)
            {
                return Result.NotFound("Not found Id");
            }
            if (user == null)
            {
                return Result.NotFound("Not found user");
            }
            rate.RatingValue = 0;
            
            await _repository.UpdateAsync(rate);
            return Result.Success();


        }
        public async Task<Result> DeleteCommentAsync(string userId, int id)
        {
            var rate = await _repository.GetByIdAsync(id);

            var user = await _userManager.FindByIdAsync(userId);
            if (id == null)
            {
                return Result.NotFound("Not found Id");
            }
            if (user == null)
            {
                return Result.NotFound("Not found user");
            }
            rate.Comment = "";

            await _repository.UpdateAsync(rate);
            return Result.Success();


        }

        public async Task<Result<RatingResponseDto> >UpdateRatingAsync(string userId,RatingDto dto, int id)
        {
            var rate = await _repository.GetByIdAsync(id);
            var prod = await _productRepository.GetByIdAsync(dto.ProductId);
            var user = await _userManager.FindByIdAsync(userId);
            if (id == null)
            {
                return Result<RatingResponseDto>.NotFound("Not found Id");
            }
            if (user == null)
            {
                return Result<RatingResponseDto>.NotFound("Not found user");
            }
            if (prod == null)
            {
                return Result<RatingResponseDto>.NotFound("Not found product");
            }
            if (dto.Comment != null)
            {
                rate.Comment = dto.Comment;
            }
            if (dto.RatingValue.HasValue)
            {
                rate.RatingValue = dto.RatingValue;
            }
            await _repository.UpdateAsync(rate);

            var res = new RatingResponseDto
            {
                UserId = user.Id,
                ProductId = dto.ProductId,
                RatingValue = dto.RatingValue.HasValue?dto.RatingValue.Value:0,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };
            return Result<RatingResponseDto>.Success(res);


        }
    }
}
