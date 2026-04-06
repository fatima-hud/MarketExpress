using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Complaint.Request
{
   public class RequestAddComplaintDto
    {
        
        public int StoreId { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}
