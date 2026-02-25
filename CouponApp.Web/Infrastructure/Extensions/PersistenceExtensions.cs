using CouponApp.Application.Interfaces.Logger;
using CouponApp.Persistence;
using CouponApp.Persistence.Contexts;
using CouponApp.Persistence.Logger;
using Microsoft.EntityFrameworkCore;

namespace CouponApp.Web.Infrastructure.Extensions
{
    public static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DiscountManagementContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString(nameof(ConnectionString.DefaultConnection))));

            services.AddScoped<IErrorLogger, DbErrorLogger>();

            return services;
        }
    }
}
