using MarketExpress.Application.Dto.PostReaction.Request;
using MarketExpress.Application.Dto.PostReaction.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Services.PostReactions
{
   public interface IPostReactionService
    {
        Task<Result<ResponseGetPostReactionDto>> AddReactionAsync(string userId, RequestAddReactionDto request);
        
    }
}
