namespace CouponApp.Web.Models.Offer
{
    public class OfferListItemViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public decimal OriginalPrice { get; set; }

        public decimal DiscountedPrice { get; set; }

        public int DiscountPercent =>
            (int)Math.Round((1 - DiscountedPrice / OriginalPrice) * 100);

        public DateTime EndDate { get; set; }

        public string MerchantName { get; set; }
    }
}
