using CouponApp.Web.Infrastructure.Extensions;
using CouponApp.Web.Infrastructure.Extensions.Auth;
using CouponApp.Web.Infrastructure.Extensions.InfrastructureExtensions;
using CouponApp.Web.Middlewares;
using Serilog;

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
                .AddFactories()
                .AddApplicationServices()
                .AddCloudinaryServices(builder.Configuration);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseErrorHandling();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSecurityMiddleware();

            app.UseDefaultRouting();

            app.UseSerilogRequestLogging();

            await app.SeedDatabaseAsync();

            app.Run();
        }
    }
}
