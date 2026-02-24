using CouponApp.Web.Infrastructure.Extensions;
using CouponApp.Web.Infrastructure.Extensions.Auth;

namespace CouponApp.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddControllersWithViews()
                .Services
                .AddMappingConfiguration()
                .AddPersistence(builder.Configuration)
                .AddIdentityConfiguration()
                .AddAuthenticationConfiguration()
                .AddRepositories()
                .AddApplicationServices();

            var app = builder.Build();

            app.UseErrorHandling();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSecurityMiddleware();

            app.UseDefaultRouting();

            await app.SeedDatabaseAsync();

            app.Run();
        }
    }
}
