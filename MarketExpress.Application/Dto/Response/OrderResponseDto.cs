using MarketExpress.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketExpress.Application.Dto.Response
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int StoreId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public enOrderStatue Statue { get; set; }
    }
}
