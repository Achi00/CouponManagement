using CouponApp.Application.Interfaces;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Infrastructure.Auth;
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

            return services;
        }
    }
}
