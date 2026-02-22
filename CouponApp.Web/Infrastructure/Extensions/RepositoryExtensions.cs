using CouponApp.Application.Interfaces.Repositories;
using CouponApp.Infrastructure.Repositories.Categories;
using CouponApp.Infrastructure.Repositories.Coupons;
using CouponApp.Infrastructure.Repositories.Merchants;
using CouponApp.Infrastructure.Repositories.Offers;
using CouponApp.Infrastructure.Repositories.Reservations;
using CouponApp.Infrastructure.Repositories.SystemSettings;

namespace CouponApp.Web.Infrastructure.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<ISystemSettingsRepository, SystemSettingsRepository>();

            return services;
        }
    }
}
