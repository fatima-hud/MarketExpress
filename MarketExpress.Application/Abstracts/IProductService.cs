using MarketExpress.Application.Dto.Product.Request;
using MarketExpress.Application.Dto.Product.Response;
using MarketExpress.Application.Dto.Request;
using MarketExpress.Application.Dto.Response;
using MarketExpress.Core.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Abstracts
{
   public interface IProductService
    {
        Task<Result<List<ResponseProductDetailsDto>>> SearchAsync(string Query);
        Task<Result<List<ResponseProductDetailsDto>>> FilterAsync(FilterDto dto);
        Task<Result<ResponseProductDetailsDto>> AddAsync(string userId,RequestAddProductDto request);
        Task<Result<ResponseUpdateProductDto>> UpdateAsync(string userId, RequestUpdateProductDto request,int productId);
        Task<Result>DeleteAsync(string userId, int id);
        Task<Result<ResponseProductDetailsDto>> GetProductAsync(int productId);

     
    }
}
