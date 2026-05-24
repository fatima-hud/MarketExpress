using MarketExpress.Application.Dto.Rating.Request;
using MarketExpress.Application.Dto.Rating.Response;
using MarketExpress.Core.Results;

namespace MarketExpress.Application.Abstracts
{
    public interface IRatingService
    {
        Task<Result<ResponseRatingDto>> AddRatingAsync(string userId, RequestRatingDto dto);

       
    
    }
}
