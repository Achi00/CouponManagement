using Mapster;

namespace CouponApp.API.Infrastructure.Extensions
{
    public static class MappingExtensions
    {
        public static IServiceCollection AddMappingConfiguration(this IServiceCollection services)
        {
            var config = new TypeAdapterConfig();
            config.Scan(typeof(Application.Mapping.MapConfig).Assembly);
            config.Scan(typeof(API.Mapping.MapConfig).Assembly);
            services.AddSingleton(config);
            return services;
        }
    }
}
