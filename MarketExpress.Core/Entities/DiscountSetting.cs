using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entities
{
    public class DiscountSetting
    {
        public int Id {  get; set; }
        public decimal DiscountPercentage {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<DiscountSetting> Discounts { get; set; }
    }
}
