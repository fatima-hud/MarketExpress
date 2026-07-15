using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.StoreFollower.Response
{
   public  class ResponseGetStoreFollowerDto
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public bool IsFollow { get; set; }
    }
}
