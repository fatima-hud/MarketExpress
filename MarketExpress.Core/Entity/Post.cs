using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.Entity
{
    public class Post
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public enPostVisibility Visibility { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<PostMedia> PostMedias { get; set; }
        public ICollection<PostReaction> PostReactions { get; set; }
        public Store Store { get; set; }

    }
}