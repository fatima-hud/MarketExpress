using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Core.DtoHelping.Admin.Response
{
    public class ResponseDashboardSummaryDto
    {
        public int ProductsCount { get; set; }
        public int FollowersCount { get; set; }
        public int PostsCount { get; set; }
        public int TotalReactions { get; set; }
    }
}
