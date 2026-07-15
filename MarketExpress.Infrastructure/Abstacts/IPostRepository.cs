using MarketExpress.Core.Entity;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
    public interface IPostRepository:IGenericRepositoryAsync<Post>
    {
        Task<List<Post>> GetAllPostsByStoreAsync(int storeId);
    }
}
