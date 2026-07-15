using MarketExpress.Application.Dto.PostMedia.Request;
using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Post.Request
{
    public class RequestAddPostDto
    {
        [Required]
        public int StoreId { get; set; }
        public string? Content { get; set; }
      
        public enPostVisibility Visibility { get; set; }
        public List<RequestAddPostMediaDto>? mediaDtos {  get; set; }


    }
}
