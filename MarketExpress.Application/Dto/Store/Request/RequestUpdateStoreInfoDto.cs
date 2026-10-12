using MarketExpress.Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Store.Request
{
    public class RequestUpdateStoreInfoDto
    {
        [Required]
       public int StoreId { get; set; }
        public string? StoreName { get; set; }
        public string? Description { get; set; } 
        public string? Address { get; set; }
       // public decimal? Latitude { get; set; }
     //   public decimal? Longitude { get; set; } 
        public TimeSpan? WorkingHoursStart { get; set; }

        public TimeSpan? WorkingHoursEnd { get; set; }
        public string?PhoneNumber { get; set; }
       

       
    }
}
