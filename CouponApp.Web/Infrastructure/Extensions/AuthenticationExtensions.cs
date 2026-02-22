using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Application.Services.Auth;
using CouponApp.Infrastructure.Auth;

namespace CouponApp.Web.Infrastructure.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services)
        {
            services.AddAuthentication().AddCookie();
            services.AddAuthorization();

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();

            return services;
        }
    }
}
