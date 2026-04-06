using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstacts
{
   public interface IRatingService
    {
        Task<Result<RatingResponseDto>>AddRatingAsync(string userId,RatingDto dto);
        Task<Result<RatingResponseDto>>UpdateRatingAsync(string userId, RatingDto dto,int id);
        Task<Result>DeleteRatingAsync(string userId,int id);
        Task<Result> DeleteCommentAsync(string userId, int id);
    }
}
