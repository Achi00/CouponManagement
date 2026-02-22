using FluentValidation;
using CouponApp.Application.DTOs.Offers;

namespace CouponApp.Application.Validators.Offers
{
    public class UpdateOfferValidator : AbstractValidator<UpdateOfferRequest>
    {
        public UpdateOfferValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.OriginalPrice)
                .GreaterThan(0);

            RuleFor(x => x.DiscountedPrice)
                .GreaterThan(0)
                .LessThan(x => x.OriginalPrice);

            RuleFor(x => x.TotalCoupons)
                .GreaterThan(0);

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate);
        }
    }
}
