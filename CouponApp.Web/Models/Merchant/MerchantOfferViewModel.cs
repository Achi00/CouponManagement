using CouponApp.Domain.Enums;

namespace CouponApp.Web.Models.Merchant
{
    public class MerchantOfferViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public OfferStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal DiscountedPrice { get; set; }
    }
}
