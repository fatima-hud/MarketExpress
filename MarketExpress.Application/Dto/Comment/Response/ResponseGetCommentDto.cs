using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Comment.Response
{
   public class ResponseGetCommentDto
    {
        public int CommentId { get; set; }

      

        public string UserId { get; set; }

        public string UserFullName { get; set; }
        public string UserImage { get; set; }
        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
