using CouponApp.Domain.Enums;

namespace CouponApp.Domain.Entity
{
    public class Offer
    {
        public Guid Id { get; set; }

        public Guid MerchantId { get; set; }
        public Guid CategoryId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public decimal OriginalPrice { get; set; }
        public decimal DiscountedPrice { get; set; }

        public int TotalCoupons { get; set; }
        public int RemainingCoupons { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public OfferStatus Status { get; set; }
        public string? RejectionReason { get; set; }

        public Merchant Merchant { get; set; }
        public Category Category { get; set; }

        public ICollection<Coupon> Coupons { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }

}
