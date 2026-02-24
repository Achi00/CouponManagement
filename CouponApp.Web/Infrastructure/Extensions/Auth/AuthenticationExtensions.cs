using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Application.Services.Auth;
using CouponApp.Infrastructure.Auth;
using CouponApp.Persistence.Identity;
using Microsoft.AspNetCore.Identity;

namespace CouponApp.Web.Infrastructure.Extensions.Auth
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services)
        {
            services.AddAuthentication().AddCookie();
            services.AddAuthorization();

            //extension for policies
            services.AddAppAuthorization();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();

            return services;
        }
    }
}
