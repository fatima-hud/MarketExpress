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

namespace MarketExpress.Application.Abstacts
{
   public interface IProductService
    {
        Task<Result<List<ProductResponse>>> SearchAsync(string Query);
        Task<Result<List<ProductResponse>>> FilterAsync(FilterDto dto);
        Task<Result<ProductResponse>> AddAsync(string userId,ProductRequest request);
        Task<Result<ProductResponse>> UpdateAsync(string userId, UpdateProductRequest request,int productId);
        Task<Result>DeleteAsync(string userId, int id);
        Task<Result<ProductResponse>> GetProductAsync(int productId);
     
    }
}
