namespace MarketExpress.Core.DtoHelping.Admin.Response
{
    public class ResponseGetProductsByAdminDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public double RatingValue { get; set; }

        public decimal? DiscountPercentage { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public decimal? PriceAfterDiscount { get; set; }

        public string Image { get; set; }
        public List<ResponseGetAllColorByAdminDto>? Colors { get; set; }

        public int TotalStock { get; set; }


        public int SoldCount { get; set; }
        public decimal SoldTotalPrice { get; set; }
    }
}
