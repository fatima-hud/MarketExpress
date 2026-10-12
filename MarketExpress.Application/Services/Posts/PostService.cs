using MarketExpress.Application.Dto.Post.Request;
using MarketExpress.Application.Dto.Post.Response;
using MarketExpress.Application.Dto.PostMedia.Response;
using MarketExpress.Application.Dto.PostReaction.Response;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Application.Services.Images;
using MarketExpress.Application.Services.Videos;
using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.Abstacts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Services.Posts
{
    public class PostService : IPostService
    {
        private readonly UserManager<User> _userManager;
        private readonly IStoreRepository _storeRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleRepository _roleRepository;
        private readonly IImageService _imageService;
        private readonly IPostMediaRepository _postMediaRepository;
        private readonly IPostRepository _postRepository;
        private readonly IVideoService _videoService;
        private readonly IPostReactionRepository _postReactionRepository;

        public PostService(UserManager<User> userManager, IStoreRepository storeRepository, RoleManager<IdentityRole> roleManager, IRoleRepository roleRepository,IImageService imageService,IPostRepository postRepository,IPostMediaRepository postMediaRepository,IVideoService videoService,IPostReactionRepository postReactionRepository)
        {
            _userManager = userManager;
            _storeRepository = storeRepository;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _imageService = imageService;
            _postMediaRepository = postMediaRepository;
            _postRepository = postRepository;
            _videoService = videoService;
            _postReactionRepository = postReactionRepository;
        }

        public async Task<Result<ResponseGetPostDto>> AddAsync(string userId, RequestAddPostDto request)
        {
            var trans = await _postRepository.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null || user.IsDeleted)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseGetPostDto>.NotFound("المستخدم غير موجود");
                }
               
                var store = await _storeRepository.GetStoreByAdminAsync(userId);
                if (store == null)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseGetPostDto>.NotFound("المتجر غير موجود");
                }
               
                var post = new Post
                {
                    Content = request.Content,
                    CreatedAt = DateTime.UtcNow,
                    StoreId = store.Id,
                    Visibility = request.Visibility

                };
                await _postRepository.AddAsync(post);
                var postMedias = new List<PostMedia>();
                if (request.mediaDtos != null && request.mediaDtos.Any())
                {

                    foreach (var media in request.mediaDtos)
                    {
                        Result<string> uploadResult;

                        if (media.MediaType == enMediaType.Image)
                        {
                            uploadResult = await _imageService.UploadImageAsync(media.File);
                        }
                        else
                        {
                            uploadResult = await _videoService.UploadVideoAsync(media.File);
                        }

                        if (!uploadResult.IsSuccess)
                        {
                            await trans.RollbackAsync();
                            return Result<ResponseGetPostDto>.BadRequest(uploadResult.Message);
                        }

                        postMedias.Add(new PostMedia
                        {
                            PostId = post.Id,
                            MediaUrl = uploadResult.Value,
                            MediaType = media.MediaType,
                            Duration = media.MediaType == enMediaType.Video
                               ? media.Duration :null

                        });
                    }
                    await _postMediaRepository.AddRangeAsync(postMedias);
                }
                await trans.CommitAsync();
                var retMedias = postMedias.Select(e => new ResponseGetPostMediaDto {
                    postMediaId = e.Id,
                    Duration = e.Duration,
                    MediaType = e.MediaType,
                    MediaUrl = e.MediaUrl
                }).ToList();
                var ret = new ResponseGetPostDto
                {
                    Id = post.Id,
                    StoreId = post.StoreId,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt,
                    Visibility = post.Visibility,
                    UpdatedAt = post.UpdatedAt,

                    IsDeleted = post.IsDeleted,
                    PostMedias = retMedias

                };
                return Result<ResponseGetPostDto>.Success(ret);

            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<ResponseGetPostDto>.Failure("خطا داخلي", failureType: ResultFailureType.InternalError);
            }
        }
        

        public async Task<Result> DeleteAsync(string userId, int postId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
            {
                return Result.NotFound("المستخدم غير موجود");
            }
            var store = await _storeRepository.GetStoreByAdminAsync(userId);
            if (store == null)
            {
              
                return Result.NotFound("المتجر غير موجود");
            }
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null)
            {
                return Result.NotFound("المنشور غير موجود");
            }
         
            if (store.Id!= post.StoreId)
            {

                return Result.BadRequest("المستخدم ليس لديه صلاحية اضافة منشور");
            }
           
            post.IsDeleted = true;
            await _postRepository.SaveChangesAsync();
            return Result.Success("تم الحذف بنجاح ");



        }

        public async Task<Result<List<ResponseGetPostDto>>> GetAllAsync(string?userId,int storeId)
        {
            var store=await _storeRepository.GetByIdAsync(storeId);
            if (store == null)
            {
                return Result<List<ResponseGetPostDto>>.NotFound("المتجر غير موجود");
            }
           var posts=await _postRepository.GetAllPostsByStoreAsync(userId,storeId);
           var ret = new List<ResponseGetPostDto>();
            if (!posts.Any())
            {
                return Result<List<ResponseGetPostDto>>.Success(ret);
            }
          
             
            ret = posts.Select(e => new ResponseGetPostDto
            {
                Id = e.Id,
                Content = e.Content,
                CreatedAt = e.CreatedAt,
                IsDeleted = e.IsDeleted,
                StoreId = storeId,
                UpdatedAt = e.UpdatedAt,
                Visibility = e.Visibility,
                MyReaction = userId!=null?e.PostReactions?.FirstOrDefault(p => p.UserId == userId && p.IsReacted)?.ReactionType:null,
                PostReactions = e.PostReactions?.Where(p=>p.IsReacted).GroupBy(r => r.ReactionType)
    .Select(g => new ResponseGetReactionSummaryDto
    {
        ReactionType = g.Key,
        Count = g.Count()
    
    
            }).ToList() ?? new List<ResponseGetReactionSummaryDto>(),
                PostMedias = e.PostMedias?
    .Select(p => new ResponseGetPostMediaDto
    {
        postMediaId = p.Id,
        MediaUrl = p.MediaUrl,
        MediaType = p.MediaType,
        Duration = p.Duration
    }).ToList() ?? new List<ResponseGetPostMediaDto>()


            }).ToList();

            return Result<List<ResponseGetPostDto>>.Success(ret);

                 }

        public async Task<Result<ResponseGetPostDto>> UpdateAsync(string userId,int postId, RequestUpdatePostDto request)
        {
            var trans=await _postMediaRepository.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null || user.IsDeleted)
                {
                    await trans.RollbackAsync();

                    return Result<ResponseGetPostDto>.NotFound("المستخدم غير موجود");
                }
                var store = await _storeRepository.GetStoreByAdminAsync(userId);
                if (store == null)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseGetPostDto>.NotFound("المتجر غير موجود");
                }

                var post = await _postRepository.GetByIdAsync(postId);
                if (post == null)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseGetPostDto>.NotFound("المنشور غير موجود");
                }
              
                if (store.Id != post.StoreId)
                {
                    await trans.RollbackAsync();
                    return Result<ResponseGetPostDto>.BadRequest("المستخدم ليس لديه صلاحية تعديل منشور");
                }
                var imageRemove = new List<PostMedia>();
                if (request.DeletedMediaIds!= null && request.DeletedMediaIds.Any())
                {
                   

                    foreach (var item in request.DeletedMediaIds)
                    {
                        var media = await _postMediaRepository.GetByIdAsync(item);
                        if (media == null)
                        {
                            await trans.RollbackAsync();
                            return Result<ResponseGetPostDto>.NotFound("احد الوسائط غير موجود");
                        }
                        if (media.PostId != postId)
                        {
                            await trans.RollbackAsync();
                            return Result<ResponseGetPostDto>.BadRequest("ليس لديك صلاحية حذف الوسائط");
                        }
                        imageRemove.Add(media);
                    }
                    await _postMediaRepository.DeleteRangeAsync(imageRemove);
                    foreach (var media in imageRemove)
                    {
                        if (media.MediaType == enMediaType.Image)
                            await _imageService.DeleteImageAsync(media.MediaUrl);
                        else
                            await _videoService.DeleteVideoAsync(media.MediaUrl);
                    }
                }
                var postMedias = new List<PostMedia>();
                if (request.NewMedias != null && request.NewMedias.Any())
                {

                    foreach (var media in request.NewMedias)
                    {
                        Result<string> uploadResult;

                        if (media.MediaType == enMediaType.Image)
                        {
                            uploadResult = await _imageService.UploadImageAsync(media.File);
                            Console.WriteLine($"URL = {uploadResult.Value}");
                        }
                        else
                        {
                            uploadResult = await _videoService.UploadVideoAsync(media.File);
                        }

                        if (!uploadResult.IsSuccess)
                        {
                            await trans.RollbackAsync();
                            return Result<ResponseGetPostDto>.BadRequest(uploadResult.Message);
                        }

                        postMedias.Add(new PostMedia
                        {
                            PostId = post.Id,
                            MediaUrl = uploadResult.Value,
                            MediaType = media.MediaType,
                            Duration = media.MediaType == enMediaType.Video
                               ? media.Duration : null

                        });
                    }
                    await _postMediaRepository.AddRangeAsync(postMedias);


                }
                if (request.Visibility.HasValue)
                {
                    post.Visibility = request.Visibility.Value;
                }
                if (!string.IsNullOrEmpty(request.Content))
                {
                    post.Content = request.Content;
                }
                post.UpdatedAt = DateTime.UtcNow;
                await _postMediaRepository.SaveChangesAsync();
                

                await trans.CommitAsync();
                var medias= await _postMediaRepository.GetAllMediasByPostAsync(postId);
                var retMedias= new List<ResponseGetPostMediaDto>();
                if (medias.Any())
                {


                    retMedias = medias.Select(e => new ResponseGetPostMediaDto
                    {
                        postMediaId = e.Id,
                        Duration = e.Duration,
                        MediaType = e.MediaType,
                        MediaUrl = e.MediaUrl
                    }).ToList();
                }
                var ret = new ResponseGetPostDto
                {
                    Id = post.Id,
                    Content = post.Content,
                    IsDeleted = post.IsDeleted,
                    StoreId = post.StoreId,
                    Visibility = post.Visibility,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt,
                    PostMedias = retMedias

                };
                return Result<ResponseGetPostDto>.Success(ret);


            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return Result<ResponseGetPostDto>.Failure("خطا داخلي", failureType: ResultFailureType.InternalError);
            }

        }
    }
}
