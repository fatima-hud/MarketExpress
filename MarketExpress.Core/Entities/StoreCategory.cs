using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entities
{
    public class StoreCategory
    {
        public int Id { get; set; }
        public int StoreId {  get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Store Store {  get; set; }
        public Category Category { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
