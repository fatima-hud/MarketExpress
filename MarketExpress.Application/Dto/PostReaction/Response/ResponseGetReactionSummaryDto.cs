using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.PostReaction.Response
{
   public class ResponseGetReactionSummaryDto
    {
        public int Count { get; set; }
        public enReactionType ReactionType { get; set; }
    }
}
