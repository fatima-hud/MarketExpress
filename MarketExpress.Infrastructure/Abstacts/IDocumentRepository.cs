using MarketExpress.Core.Entity;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Abstacts
{
    public interface IDocumentRepository:IGenericRepositoryAsync<Document>
    {
        Task<Document> GetStoreDocumentByNationalIdFrontImageAsync(int storeId);
        Task<Document> GetStoreDocumentByNationalIdBackImageAsync(int storeId);
        Task<Document> GetStoreDocumentByStoreLicenseImageAsync(int storeId);
    }
}
