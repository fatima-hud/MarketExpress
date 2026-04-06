using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Complaint.Response
{
   public class ResponseGetComplaintDto
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerImageUrl { get; set; }


        public string StoreName { get; set; }
        public string? StoreLogoUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

