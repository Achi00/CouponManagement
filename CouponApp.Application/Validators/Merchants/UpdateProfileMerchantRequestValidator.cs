using CouponApp.Application.DTOs.Merchant;
using FluentValidation;

namespace CouponApp.Application.Validators.Merchants
{
    public class UpdateProfileMerchantRequestValidator : AbstractValidator<UpdateProfileMerchantRequest>
    {
        public UpdateProfileMerchantRequestValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.BusinessName)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
