namespace CouponApp.Application.DTOs.Offers
{
    public class CreateOfferRequest
    {
        public Guid CategoryId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public decimal OriginalPrice { get; set; }
        public decimal DiscountedPrice { get; set; }

        public int TotalCoupons { get; set; }
        public int RemainingCoupons { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ImageUrl { get; set; }
    }
}
