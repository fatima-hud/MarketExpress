namespace MarketExpress.Core.DtoHelping.Admin.Response
{
    public class ResponseGetAllColorByAdminDto
    {
        public int ProductColorId { get; set; }
        public string Color { get; set; }
        public string ColorHexCode { get; set; }

        public List<ResponseGetAllSizeByAdminDto>? Sizes { get; set; }
    }
}
