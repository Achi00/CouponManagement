using CouponApp.Domain.Enums;
namespace CouponApp.Domain.Entity
{
    public class Coupon
    {
        public Guid Id { get; set; }

        public Guid OfferId { get; set; }
        public Guid UserId { get; set; }

        public string Code { get; set; }
        public CouponStatus Status { get; set; }

        public DateTime PurchasedAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public DateTime ExpirationDate { get; set; }

        public Offer Offer { get; set; }
        public User User { get; set; }
    }
}
