using MarketExpress.Application.Dto.PostMedia.Response;
using MarketExpress.Application.Dto.PostReaction.Response;
using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Post.Response
{
    public class ResponseGetPostDto
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public enPostVisibility Visibility { get; set; }
        public bool IsDeleted { get; set; }
        public List<ResponseGetPostMediaDto> PostMedias { get; set; }
        public enReactionType? MyReaction { get; set; }   
        public List <ResponseGetReactionSummaryDto> PostReactions { get; set; }
    }
}
