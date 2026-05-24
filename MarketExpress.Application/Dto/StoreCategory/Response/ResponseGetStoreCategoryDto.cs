using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.StoreCategory.Response
{
   public class ResponseGetStoreCategoryDto
    {
        public int Id { get; set; }
        public int StoreId {  get; set; }
        public int CategoryId { get; set; }
       public DateTime CreatedAt {  get; set; }
              
    }
}
