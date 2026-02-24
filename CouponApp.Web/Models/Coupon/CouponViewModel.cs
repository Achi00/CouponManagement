using CouponApp.Domain.Enums;

namespace CouponApp.Web.Models.Coupon
{
    public class CouponViewModel
    {
        public Guid Id { get; set; }

        public string OfferTitle { get; set; }

        public string Code { get; set; }

        public DateTime CreatedAt { get; set; }

        public CouponStatus Status { get; set; }
    }
}
