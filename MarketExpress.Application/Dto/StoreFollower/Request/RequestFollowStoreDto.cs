using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.StoreFollower.Request
{
   public class RequestFollowStoreDto
    {
        [Required]
        public int StoreId { get; set; }
    
    }
}
