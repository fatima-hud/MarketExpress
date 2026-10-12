using MarketExpress.Application.Dto.Register.Request;
using MarketExpress.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.RequestStore.Request
{
    public class RequestStoreDto
    {


        [Required]
        public string StoreName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Address { get; set; }

        [Required]
        public IFormFile FeaturedImage { get; set; }

        [Required]
        public IFormFile Logo { get; set; }

      
     //   public decimal Latitude { get; set; }
       
      //  public decimal Longitude { get; set; }
        [Required]
        public TimeSpan WorkingHoursStart { get; set; }
        [Required]
        public TimeSpan WorkingHoursEnd { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public IFormFile NationalIdFrontImage { get; set; }
        [Required]
        public IFormFile NationalIdBackImage { get; set; }
        [Required]
        public IFormFile StoreLicense { get; set; }




    }
}

