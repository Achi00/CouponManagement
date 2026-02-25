using Microsoft.AspNetCore.Identity;
using CouponApp.Persistence.Contexts;
using CouponApp.Persistence.Identity;

namespace CouponApp.API.Infrastructure.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<DiscountManagementContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
