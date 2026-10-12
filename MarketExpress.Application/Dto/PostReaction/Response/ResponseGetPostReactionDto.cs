using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.PostReaction.Response
{
   public class ResponseGetPostReactionDto
    {
        public int Id { get; set; }
      
       public int ReactionCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public enReactionType ReactionType { get; set; }
        public bool IsReacted { get; set; }
      

    }
}
