using CouponApp.API.Constants;

namespace CouponApp.API.Infrastructure.Extensions.Auth
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("MerchantOnly",
                    policy => policy.RequireRole(AppRoles.Merchant));

                options.AddPolicy("AdminOnly",
                    policy => policy.RequireRole(AppRoles.Admin));

                options.AddPolicy("CustomerOnly",
                    policy => policy.RequireRole(AppRoles.Customer));
            });

            return services;
        }
    }
}
