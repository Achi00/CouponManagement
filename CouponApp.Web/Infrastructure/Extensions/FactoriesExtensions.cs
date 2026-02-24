using CouponApp.Web.Factories;

namespace CouponApp.Web.Infrastructure.Extensions
{
    public static class FactoriesExtensions
    {
        public static IServiceCollection AddFactories(this IServiceCollection services)
        {
            services.AddScoped<CreateOfferViewModelFactory>();

            return services;
        }
    }
}
