using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.DtoHelping.Admin.Response
{
 public   class ResponseGetAllSizeByAdminDto
    {
        public int ProductSizeId { get; set; }
        public enSize Size { get; set; }
        public int Quantity { get; set; }
        public bool IsFoundProduct { get; set; }
    }
}
