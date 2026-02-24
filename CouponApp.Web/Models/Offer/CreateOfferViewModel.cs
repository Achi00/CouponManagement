using System.ComponentModel.DataAnnotations;

namespace CouponApp.Web.Models.Offer
{
    public class CreateOfferViewModel : IValidatableObject
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [StringLength(120)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Range(0.01, 100000)]
        public decimal OriginalPrice { get; set; }

        [Range(0.01, 100000)]
        public decimal DiscountedPrice { get; set; }

        [Range(1, 100000)]
        public int TotalCoupons { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DiscountedPrice >= OriginalPrice)
            {
                yield return new ValidationResult(
                    "Discounted price must be lower than original price",
                    new[] { nameof(DiscountedPrice) });
            }

            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "End date must be after start date",
                    new[] { nameof(EndDate) });
            }
        }
    }
}
