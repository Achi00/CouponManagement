using System.ComponentModel.DataAnnotations;

namespace CouponApp.Web.Models.Offer
{
    public class OfferDetailsViewModel
    {
        public Guid Id { get; set; }

        [Required, StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required, StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal OriginalPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DiscountedPrice { get; set; }

        [Range(0, int.MaxValue)]
        public int RemainingCoupons { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        public string MerchantName { get; set; } = string.Empty;

        [Required]
        public string CategoryName { get; set; } = string.Empty;

        /* ---------- Computed UI properties ---------- */

        public int DiscountPercent =>
            OriginalPrice == 0
                ? 0
                : (int)Math.Round((1 - DiscountedPrice / OriginalPrice) * 100);

        public bool IsSoldOut => RemainingCoupons <= 0;

        public bool IsUrgent => RemainingCoupons <= 5 && !IsSoldOut;

        public int DaysLeft =>
            Math.Max((EndDate.Date - DateTime.UtcNow.Date).Days, 0);

        public char MerchantInitial =>
            string.IsNullOrWhiteSpace(MerchantName)
                ? 'M'
                : MerchantName[0];
    }
}
