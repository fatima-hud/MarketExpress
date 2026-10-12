using MarketExpress.Application.Dto.Post.Request;
using MarketExpress.Application.Dto.Post.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Services.Posts
{
    public interface IPostService
    {
        Task<Result<ResponseGetPostDto>> AddAsync(string userId, RequestAddPostDto request);
        Task<Result>DeleteAsync(string userId, int postId);
        Task<Result<List<ResponseGetPostDto>>> GetAllAsync(string?userId,int storeId);
        Task<Result<ResponseGetPostDto>>UpdateAsync(string userId,int postId, RequestUpdatePostDto request);
       
    }
}
