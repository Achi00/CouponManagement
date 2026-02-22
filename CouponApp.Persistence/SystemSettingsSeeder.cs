using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CouponApp.Domain.Entity;
using CouponApp.Persistence.Contexts;

namespace CouponApp.Persistence
{
    public static class SystemSettingsSeeder
    {
        public static async Task SeedSystemSettingsAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<DiscountManagementContext>();

            var cancellationToken = CancellationToken.None;

            var exists = await context.SystemSettings
                .AnyAsync(cancellationToken);

            if (exists)
                return;

            var settings = new SystemSetting { ReservationDurationMinutes = 30, MerchantEditPeriodHours = 24 };

            context.SystemSettings.Add(settings);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
