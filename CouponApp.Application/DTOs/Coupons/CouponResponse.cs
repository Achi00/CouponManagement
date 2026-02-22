using CouponApp.Domain.Enums;

namespace CouponApp.Application.DTOs.Coupons
{
    public class CouponResponse
    {
        public string Code { get; set; }

        public string OfferTitle { get; set; }

        public DateTime ExpirationDate { get; set; }

        public CouponStatus Status { get; set; }
    }
}
