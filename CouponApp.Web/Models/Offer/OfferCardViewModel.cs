namespace CouponApp.Web.Models.Offer
{
    public class OfferCardViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal DiscountedPrice { get; set; }

        public int RemainingCoupons { get; set; }

        public DateTime EndDate { get; set; }
    }
}
