using MarketExpress.Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.PostMedia.Request
{
   public class RequestAddPostMediaDto
    {
     
        public IFormFile File { get; set; }
        public enMediaType MediaType { get; set; }

        public int? Duration { get; set; }
    }
}
