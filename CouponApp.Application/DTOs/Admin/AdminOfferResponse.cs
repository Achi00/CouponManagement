namespace CouponApp.Application.DTOs.Admin
{
    public class AdminOfferResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string MerchantName { get; set; }

        public string CategoryName { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal DiscountedPrice { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Description { get; set; }

        public int RemainingCoupons { get; set; }
    }
}
