namespace CouponApp.Application.DTOs.Offers
{
    public class UpdateOfferRequest
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal DiscountedPrice { get; set; }

        public int TotalCoupons { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Guid CategoryId { get; set; }
        public string? ImageUrl { get; set; }
    }
}
