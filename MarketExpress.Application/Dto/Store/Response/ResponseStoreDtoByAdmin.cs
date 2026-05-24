using MarketExpress.Application.Dto.Request;

namespace MarketExpress.Application.Dto.Store.Response
{
    public class ResponseStoreDtoByAdmin
    {


        public int Id { get; set; }
        public string AdminId { get; set; }
        public string AdminName { get; set; }
        public string AdminUserName { get; set; }
        public string AdminEmail { get; set; }
        public string AdminPhoneNumber { get; set; }




        public string StoreName { get; set; }
        public string Description { get; set; }
        public string StorePhoneNumber { get; set; }
        public string Address { get; set; }
        public string StoreEmail { get; set; }

        public string Logo { get; set; }


        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool IsActive { get; set; }
        public bool IsOpenNow { get; set; }

        public List<BusinessHourResponseDto> BusinessHours { get; set; }

    }
}
