using MarketExpress.Application.Abstracts;
using MarketExpress.Application.Dto.Rating.Request;
using MarketExpress.Application.Dto.Rating.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Application.Implementations
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IProductRepository _productRepository;
        private readonly UserManager<User> _userManager;

        public RatingService(IRatingRepository repository, IProductRepository productRepository, UserManager<User> userManager)
        {
            _ratingRepository = repository;
            _productRepository = productRepository;
            _userManager = userManager;
        }

        public async Task<Result<ResponseRatingDto>> AddRatingAsync(string userId, RequestRatingDto request)
        {
            var prod = await _productRepository.GetByIdAsync(request.ProductId);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<ResponseRatingDto>.NotFound("Not found user");
            }
            if (user.IsBanned)
            {
                return Result<ResponseRatingDto>.BadRequest("User is banned");
            }
            if (prod == null)
            {
                return Result<ResponseRatingDto>.NotFound("Not found product");
            }
            var rating = await _ratingRepository.GetUserRatingForProductAsync(userId, request.ProductId);

            if (!request.RatingValue.HasValue)
            {
                if (rating != null)
                {
                    rating.RatingValue = null;
                    await _ratingRepository.UpdateAsync(rating);
                }

                return Result<ResponseRatingDto>.Success(null);
            }
            if (request.RatingValue <= 0 || request.RatingValue > 5)
            {
                return Result<ResponseRatingDto>.BadRequest("Rating should be greater than 0 and less than 6");

            }
            if (rating != null)
            {
                rating.RatingValue = request.RatingValue.Value;
                await _ratingRepository.UpdateAsync(rating);
            }
            else
            {


                rating = new Rating
                {
                    UserId = user.Id,
                    ProductId = request.ProductId,
                    RatingValue = request.RatingValue.Value,

                    CreatedAt = DateTime.UtcNow
                };


                await _ratingRepository.AddAsync(rating);

            }
            var res = new ResponseRatingDto
            {
                UserId = user.Id,
                ProductId = rating.ProductId,
                RatingValue = rating.RatingValue.Value,

                CreatedAt = rating.CreatedAt
            };
            return Result<ResponseRatingDto>.Success(res);


        }





    }
}
