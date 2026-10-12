using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.DtoHelping.Admin.Response
{
   public class ResponseDashboardAnalyticsDto
    {
      
        public int OrdersCount { get; set; }
        public decimal TotalSales { get; set; }
       
        public int CustomersCount { get; set; }
       

    }
}
