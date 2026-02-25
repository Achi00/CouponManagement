using CouponApp.Persistence.Identity;
using CouponApp.Persistence.Seeds;

namespace CouponApp.API.Infrastructure.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication UseErrorHandling(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            return app;
        }

        public static WebApplication UseStaticAssets(this WebApplication app)
        {
            app.UseStaticFiles();

            return app;
        }

        public static WebApplication UseSecurityMiddleware(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

        public static WebApplication UseDefaultRouting(this WebApplication app)
        {
            app.UseRouting();
            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            return app;
        }

        public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            await RoleSeeder.SeedAsync(scope.ServiceProvider);
            await app.Services.SeedSystemSettingsAsync();
            await app.Services.SeedCategoriesAsync();

            return app;
        }
    }
}
