using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
  public interface IProductSizeRepository: IGenericRepositoryAsync<ProductSize>
    {
        Task<ProductSize> GetProductSizeByProductColorAndSizeAsync(int productColorId, enSize size);
        Task<ProductSize> GetProductSizeAsync(int productId, string color,enSize size);
    }
}
