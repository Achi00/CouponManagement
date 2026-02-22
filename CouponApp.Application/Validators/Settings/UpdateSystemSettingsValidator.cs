using FluentValidation;
using CouponApp.Application.DTOs.Settings;

namespace CouponApp.Application.Validators.Settings
{
    public class UpdateSystemSettingsValidator : AbstractValidator<UpdateSystemSettingsRequest>
    {
        public UpdateSystemSettingsValidator()
        {
            RuleFor(x => x.ReservationDurationMinutes)
                .GreaterThan(0)
                .LessThan(1440);

            RuleFor(x => x.MerchantEditPeriodHours)
                .GreaterThan(0)
                .LessThan(168);
        }
    }
}
