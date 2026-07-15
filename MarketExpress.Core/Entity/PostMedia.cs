using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
   public class PostMedia
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string MediaUrl { get; set; }
        public enMediaType MediaType { get; set; }

       public int ?Duration { get; set; } 
        public Post Post { get; set; }

    }
}
