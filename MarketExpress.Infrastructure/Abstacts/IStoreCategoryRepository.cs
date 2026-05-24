using MarketExpress.Core.Entity;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
    public interface IStoreCategoryRepository:IGenericRepositoryAsync<StoreCategory>
    {
        Task<bool> IsCategoryExistByStoreAsync(int storeId, int categoryId);
        Task<List<StoreCategory>>GetAllStoreCategoryAsync(int storeId);
    }
}
