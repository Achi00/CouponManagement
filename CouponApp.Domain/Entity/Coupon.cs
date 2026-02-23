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

        private Coupon() { }

        public static Coupon Create(Guid userId, Guid offerId, string code)
        {
            return new Coupon
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                OfferId = offerId,
                Code = code,
                Status = CouponStatus.Active,
                PurchasedAt = DateTime.UtcNow
            };
        }
    }
}
