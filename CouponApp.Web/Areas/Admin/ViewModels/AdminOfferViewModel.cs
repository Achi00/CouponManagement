namespace CouponApp.Web.Areas.Admin.ViewModels
{
    public class AdminOfferViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string MerchantName { get; set; }

        public string CategoryName { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal DiscountedPrice { get; set; }

        public int RemainingCoupons { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
