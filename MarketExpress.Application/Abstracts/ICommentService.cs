using MarketExpress.Application.Dto.Comment.Request;
using MarketExpress.Application.Dto.Comment.Response;
using MarketExpress.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstracts
{
   public interface ICommentService
    {
        Task<Result<ResponseAddCommentDto>> AddAsync(string userId,RequestAddCommentDto request);
        Task<Result<List<ResponseGetCommentDto>>> GetAllAsync( RequestGetAllCommentDto request );
        Task<Result> DeleteAsync(string userId,int commentId);
        Task<Result<ResponseAddCommentDto>>UpdateAsync(string userId,RequestUpdateCommentDto request);


    }
}
