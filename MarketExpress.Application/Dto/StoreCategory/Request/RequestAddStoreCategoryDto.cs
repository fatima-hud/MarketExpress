using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.StoreCategory.Request
{
    public class RequestAddStoreCategoryDto
    {
       
        [Required]
        public int CategoryId {  get; set; }
    }
}
