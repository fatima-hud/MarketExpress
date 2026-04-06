using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.RequestStore.Response
{
   public class ResponsePendingStoreDto
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public string StoreName { get; set; }
        public string Description { get; set; }

        public string Address { get; set; }

        public string? Logo { get; set; }
        public string? FeaturedImage { get; set; }

        public bool IsActive { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public TimeSpan WorkingHoursStart { get; set; }

        public TimeSpan WorkingHoursEnd { get; set; }
        public DateTime CreatedAt { get; set; }
        public string NationalIdFrontImage { get; set; }
        public string NationalIdBackImage { get; set; } 


        public enStoreStatus StoreStatus { get; set; }
      
    }
}
