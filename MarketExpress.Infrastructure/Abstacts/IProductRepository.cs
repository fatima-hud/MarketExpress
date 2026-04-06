using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Core.Results;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
   public interface IProductRepository:IGenericRepositoryAsync<Product>
    {
        Task<List<Product>> SearchAsync(string Query);
        Task<List<Product>> FilterAsync(decimal?minPrice, decimal?maxPrice,enType? Type,string?Color,string? Size);
    
     
        



    }
}
