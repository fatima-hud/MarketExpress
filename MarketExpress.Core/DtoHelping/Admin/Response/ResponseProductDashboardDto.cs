using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.DtoHelping.Admin.Response
{
   public class ResponseProductDashboardDto
    {
        public List<ResponseGetProductsByAdminDto> Products {  get; set; }
        public int TotalProductsCount {  get; set; }
    }
}
