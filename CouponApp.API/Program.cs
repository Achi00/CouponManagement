
using CouponApp.API.Infrastructure.Extensions;
using CouponApp.API.Infrastructure.Extensions.Auth;
using CouponApp.API.Middlewares;
using CouponApp.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;

namespace CouponApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services
            .AddControllers()
            .Services
            .AddEndpointsApiExplorer()
            .AddMappingConfiguration()
            .AddSwaggerConfiguration()
            .AddPersistence(builder.Configuration)
            .AddIdentityConfiguration()
            .AddAuthConfiguration(builder.Configuration)
            .AddRepositories()
            .AddApplicationServices();

            // health checks
            builder.Services.AddHealthChecks();
            // for future
            //builder.Services.AddHealthChecks()
            //    .AddDbContextCheck<DiscountManagementContext>();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwaggerConfiguration();
            app.UseMiddleware<ApiExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseRateLimiterConfiguration();
            app.UseAuthorization();
            app.MapControllers().RequireRateLimiting("api");
            app.UseSerilogRequestLogging();
            app.MapHealthChecks("/health");

            app.Run();
        }
    }
}
