using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.RequestStore.Response
{
   public class ResponseRejectedStoreRequestDto
    {
        public int RequestId { get; set; }
        public string StoreName { get; set; }
        public enStoreStatus status { get; set; }
        public DateTime RejectedAt { get; set; }
        public string Note { get; set; }
    }
}
