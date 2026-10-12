using MarketExpress.Application.Dto.PostReaction.Request;
using MarketExpress.Application.Dto.PostReaction.Response;
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

namespace MarketExpress.Application.Services.PostReactions
{
    public class PostReactionService : IPostReactionService
    {
        private readonly IPostReactionRepository _postReactionRepository;
        private readonly UserManager<User> _userManager;
        private readonly IPostRepository _postRepository;

        public PostReactionService(IPostReactionRepository postReactionRepository, UserManager<User> userManager, IPostRepository postRepository)
        {
            _postReactionRepository = postReactionRepository;
            _userManager = userManager;
            _postRepository = postRepository;
        }

        public async Task<Result<ResponseGetPostReactionDto>> AddReactionAsync(string userId, RequestAddReactionDto request)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null || user.IsDeleted)
            {
                return Result<ResponseGetPostReactionDto>.NotFound("المستخدم غير موجود");
            }
            var post = await _postRepository.GetByIdAsync(request.PostId);
            if (post == null)
            {
                return Result<ResponseGetPostReactionDto>.NotFound("المنشور غير موجود");
            }
            var postReaction = await _postReactionRepository.GetPostReactionByUserIdAndPostIdAsync(userId, request.PostId);
            if (postReaction == null)
            {
                postReaction = new PostReaction
                {
                    UserId = userId,
                    PostId = request.PostId,
                    ReactionType = request.ReactionType,
                    CreatedAt = DateTime.UtcNow,
                    IsReacted = true

                };
                await _postReactionRepository.AddAsync(postReaction);
            }
            else
            {
                if (postReaction.ReactionType == request.ReactionType)
                {
                    postReaction.IsReacted = !postReaction.IsReacted;

                }
                else
                {
                    postReaction.ReactionType = request.ReactionType;
                    postReaction.IsReacted = true;

                }
                await _postReactionRepository.UpdateAsync(postReaction);
            }
            var count = await _postReactionRepository.GetPostReactionsCountByPostIdAndReactionTypeAsync(request.PostId, request.ReactionType);
            var ret = new ResponseGetPostReactionDto
            {
                Id = postReaction.Id,
                
                ReactionCount = count,
                CreatedAt = postReaction.CreatedAt,
                ReactionType = postReaction.ReactionType,
                IsReacted = postReaction.IsReacted
            };
            return Result<ResponseGetPostReactionDto>.Success(ret);



        }
    }
}
