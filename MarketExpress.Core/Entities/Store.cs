using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entities
{
   public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber {  get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public byte[] Logo { get; set; }
        public DateTime CreatedAT { get; set; }
        public bool IsActive { get; set; } 
        public Decimal Latitude {  get; set; }
        public Decimal Longitude { get; set; }
        public IEnumerable<StoreCategory>StoreCategories { get; set; }
        

    }
}
