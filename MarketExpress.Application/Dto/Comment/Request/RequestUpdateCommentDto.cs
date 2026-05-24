using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Comment.Request
{
   public class RequestUpdateCommentDto
    {
        [Required]
        public int CommentId { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
