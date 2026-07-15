using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Store.Request
{
   public class RequestUpdateStoreImagesDto
    {
        public IFormFile? FeaturedImage { get; set; }

        
        public IFormFile? Logo { get; set; }
    }
}
