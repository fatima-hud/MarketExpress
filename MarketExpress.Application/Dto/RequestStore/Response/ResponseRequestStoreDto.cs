using MarketExpress.Core.Entity;
using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.RequestStore.Response
{
    public class ResponseRequestStoreDto
    {
        public int RequestId {get;set;}
        public string StoreName { get; set; }
        public enStoreStatus status { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
