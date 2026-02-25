using CouponApp.API.Middlewares;
using CouponApp.Persistence.Contexts;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace CouponApp.API.Infrastructure.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddHealthCheckConfiguration(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<DiscountManagementContext>();
            return services;
        }

        public static WebApplication UseHealthCheckConfiguration(this WebApplication app)
        {
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckResponseWriter.WriteResponse
            });
            return app;
        }
    }
}
