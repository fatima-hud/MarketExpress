using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.PostMedia.Response
{
   public class ResponseGetPostMediaDto
    {
        public int postMediaId {  get; set; }
        public string MediaUrl { get; set; }
        public enMediaType MediaType { get; set; }

        public int? Duration { get; set; }
    }
}
