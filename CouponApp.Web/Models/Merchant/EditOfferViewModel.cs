using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CouponApp.Web.Models.Merchant
{
    public class EditOfferViewModel : IValidatableObject
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Category")]
        public Guid CategoryId { get; set; }

        [Required]
        [StringLength(120)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }

        [Range(0.01, 100000)]
        [Display(Name = "Original Price")]
        public decimal OriginalPrice { get; set; }

        [Range(0.01, 100000)]
        [Display(Name = "Discounted Price")]
        public decimal DiscountedPrice { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public string? ExistingImageUrl { get; set; }
        public IFormFile? NewImageFile { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DiscountedPrice >= OriginalPrice)
                yield return new ValidationResult(
                    "Discounted price must be lower than original price",
                    new[] { nameof(DiscountedPrice) });

            if (EndDate <= StartDate)
                yield return new ValidationResult(
                    "End date must be after start date",
                    new[] { nameof(EndDate) });
        }
    }
}
