using MarketExpress.Application.Dto.Comment.Request;
using MarketExpress.Application.Dto.Comment.Response;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;

namespace MarketExpress.Application.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly UserManager<User> _userManager;
        private readonly IProductRepository _productRepository;
        private readonly ICommentRepository _commentRepository;

        public CommentService(UserManager<User> userManager, IProductRepository productRepository, ICommentRepository commentRepository)
        {
            _userManager = userManager;
            _productRepository = productRepository;
            _commentRepository = commentRepository;
        }

        public async Task<Result<ResponseAddCommentDto>> AddAsync(string userId, RequestAddCommentDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result<ResponseAddCommentDto>.NotFound("المستخدم غير موجود");
            }
            if (user.IsBanned)
            {

                return Result<ResponseAddCommentDto>.BadRequest("المستخدم محظور");
            }
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result<ResponseAddCommentDto>.NotFound("المنتج غير موجود");
            }
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return Result<ResponseAddCommentDto>.BadRequest("محتوى التعليق مطلوب");
            }
            var newComment = new Comment
            {
                Content = request.Content.Trim(),
                UserId = userId,
                ProductId = request.ProductId,
                CreatedAt = DateTime.UtcNow,

            };
            await _commentRepository.AddAsync(newComment);
            var ret = new ResponseAddCommentDto
            {
                CommentId = newComment.Id,
                ProductId = newComment.ProductId,
                UserId = newComment.UserId,
                UserFullName = user.FullName,
                UserImage = user.ProfilePhoto,
                Text = newComment.Content,
                CreatedAt = newComment.CreatedAt,


            };
            return Result<ResponseAddCommentDto>.Success(ret);
        }

        public async Task<Result> DeleteAsync(string userId, int commentId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {

                return Result.BadRequest("المستخدم محظور");
            }

            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
            {
                return Result.NotFound("التعليق غير موجود");
            }

            var store = await _productRepository.GetStoreByProductIdAsync(comment.ProductId);
            if (store is null)
            {
                return Result.NotFound("المتجر غير موجود");
            }
            var role = await _userManager.IsInRoleAsync(user, "SuperAdmin");
            if (comment.UserId != userId && store.OwnerId != userId && !role)
            {
                return Result.BadRequest("غير مصرح لك حذف هذا التعليق");
            }
            comment.IsDeleted = true;
            comment.DeletedAt = DateTime.UtcNow;
            await _commentRepository.UpdateAsync(comment);
            return Result.Success();
        }

        public async Task<Result<List<ResponseGetCommentDto>>> GetAllAsync(RequestGetAllCommentDto request)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result<List<ResponseGetCommentDto>>.NotFound("المنتج غير موجود");
            }
            if (request.PageNumber <= 0)
                request.PageNumber = 1;

            if (request.PageSize <= 0 || request.PageSize > 50)
                request.PageSize = 10;
            var res = await _commentRepository.GetAllCommentByProduct(request.ProductId, request.PageNumber, request.PageSize);
            var ret = res.Select(c => new ResponseGetCommentDto
            {
                CommentId = c.Id,
                UserId = c.UserId,
                UserFullName = c.User.FullName,
                UserImage = c.User.ProfilePhoto,
                Text = c.Content,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt

            }).ToList();
            return Result<List<ResponseGetCommentDto>>.Success(ret);


        }

        public async Task<Result<ResponseAddCommentDto>> UpdateAsync(string userId, RequestUpdateCommentDto request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted )
            {
                return Result<ResponseAddCommentDto>.NotFound("مستخدم غير موجود");
            }
            if (user.IsBanned)
            {

                return Result<ResponseAddCommentDto>.BadRequest("المستخدم محظور");
            }
            var comment = await _commentRepository.GetByIdAsync(request.CommentId);
            if (comment == null)
            {
                return Result<ResponseAddCommentDto>.NotFound("التعليق غير موجود");
            }

            if (comment.UserId != userId)
            {
                return Result<ResponseAddCommentDto>.BadRequest("غير مصرح لك تعديل هذا التعليق");
            }

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return Result<ResponseAddCommentDto>.BadRequest("محتوى التعليق مطلوب");
            }
            comment.Content = request.Content.Trim();
            comment.UpdatedAt = DateTime.UtcNow;
            await _commentRepository.UpdateAsync(comment);
            var ret = new ResponseAddCommentDto
            {
                CommentId = comment.Id,
                ProductId = comment.ProductId,
                UserId = comment.UserId,
                UserFullName = user.FullName,
                UserImage = user.ProfilePhoto,
                Text = comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt
            };
            return Result<ResponseAddCommentDto>.Success(ret);


        }
    }
}
