using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using MarketExpress.Database;
using MarketExpress.Infrastructure.Abstacts;
using MarketExpress.Infrastructure.InfrastructureBases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Infrastructure.Repositories
{
    public class DocumentRepository : GenericRepositoryAsync<Document>, IDocumentRepository
    {
        public DocumentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    public async Task<Document> GetStoreDocumentByNationalIdBackImageAsync(int storeId)
        {
            var res = await _dbContext.Documents.FirstOrDefaultAsync(e => e.StoreId==storeId && e.FileType == enFileDocumentType.NationalIdBackImage );
            return res;
        }

       

        public async Task<Document> GetStoreDocumentByNationalIdFrontImageAsync(int storeId)
        {
            var res = await _dbContext.Documents.FirstOrDefaultAsync(e => e.StoreId == storeId && e.FileType == enFileDocumentType.NationalIdFrontImage );
            return res;
        }

        public async Task<Document> GetStoreDocumentByStoreLicenseImageAsync(int storeId)
        {
            var res = await _dbContext.Documents.FirstOrDefaultAsync(e => e.StoreId == storeId && e.FileType == enFileDocumentType.StoreLicense);
            return res;
        }
    }

      
        
    }

