using FluentValidation;
using FluentValidation.AspNetCore;

namespace CouponApp.API.Infrastructure.Extensions
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(typeof(Application.Validators.Offers.CreateOfferValidator).Assembly);

            return services;
        }
    }
}
