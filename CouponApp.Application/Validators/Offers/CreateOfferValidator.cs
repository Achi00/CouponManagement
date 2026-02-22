using FluentValidation;
using CouponApp.Application.DTOs.Offers;

namespace CouponApp.Application.Validators.Offers
{
    public class CreateOfferValidator : AbstractValidator<CreateOfferRequest>
    {
        public CreateOfferValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(2000);

            RuleFor(x => x.OriginalPrice)
                .GreaterThan(0);

            RuleFor(x => x.DiscountedPrice)
                .GreaterThan(0);

            RuleFor(x => x.DiscountedPrice)
                .LessThan(x => x.OriginalPrice);

            RuleFor(x => x.TotalCoupons)
                .GreaterThan(0);

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate);
        }
    }
}
