using MarketExpress.Core.Entity;
using MarketExpress.Infrastructure.InfrastructureBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MarketExpress.Infrastructure.Abstacts
{
    public interface ITransactionRepository:IGenericRepositoryAsync<TransactionProcess>
    {
        Task<List<TransactionProcess>> GetAllAsync(Guid walletId);
    }
}
