using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Application.Interfaces.Sercives.Offer;
using CouponApp.Application.Services;
using CouponApp.Application.Services.Offers;
using CouponApp.Infrastructure.Auth;
using CouponApp.Infrastructure.BackgroundJobs;
using CouponApp.Persistence;
using CouponApp.Persistence.Email;

namespace CouponApp.Web.Infrastructure.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailService, SmtpEmailservice>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICouponCodeGenerator, CouponCodeGenerator>();
            services.AddHostedService<ExpiredReservationWorker>();

            // application services
            services.AddScoped<IMerchantService, MerchantService>();

            services.AddScoped<IOfferQueryService, OfferService>();
            services.AddScoped<IMerchantOfferService, OfferService>();
            services.AddScoped<IAdminOfferService, AdminOfferService>();

            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISystemSettingsService, SystemSettingsService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IUserProfileService, UserProfileService>();


            return services;
        }
    }
}
