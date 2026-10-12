using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.DtoHelping.Complaint
{
   public  class ResponseGetComplaintByUserDto
    {
     
            public int ComplaintId { get; set; }
            public int StoreId { get; set; }
            public string? StoreName { get; set; }
            public string? StoreLogo { get; set; }



            public string ?Title { get; set; }
            public string? Description { get; set; }
            public DateTime CreatedAt { get; set; }
        public int NumberOfUnReadMessage { get; set; }
        public DateTime? LastMessageAt { get; set; }


    }
}
