using MarketExpress.Application.Dto.Rating.Request;
using MarketExpress.Application.Dto.Rating.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Services.Ratings
{
    public interface IRatingService
    {
        Task<Result<ResponseRatingDto>> AddRatingAsync(string userId, RequestRatingDto dto);
        Task<Result<int>> GetProductRatingByUserAsync(string userId, int productId);



    }
}
