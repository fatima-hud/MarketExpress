using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class PostReaction
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public  enReactionType ReactionType{ get; set; }
        public User User { get; set; }
        public Post Post { get; set; }

    }
}
