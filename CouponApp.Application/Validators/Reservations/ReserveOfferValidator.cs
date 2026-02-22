using FluentValidation;
using CouponApp.Application.DTOs.Reservations;

namespace CouponApp.Application.Validators.Reservations
{
    public class ReserveOfferValidator : AbstractValidator<ReserveOfferRequest>
    {
        public ReserveOfferValidator()
        {
            RuleFor(x => x.OfferId)
                .NotEmpty();

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .LessThanOrEqualTo(10);
        }
    }
}
