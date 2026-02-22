using Microsoft.EntityFrameworkCore;
using CouponApp.Persistence;
using CouponApp.Persistence.Contexts;

namespace CouponApp.Web.Infrastructure.Extensions
{
    public static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DiscountManagementContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString(nameof(ConnectionString.DefaultConnection))));

            return services;
        }
    }
}
