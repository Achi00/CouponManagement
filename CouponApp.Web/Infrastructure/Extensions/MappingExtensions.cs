using Mapster;

namespace CouponApp.Web.Infrastructure.Extensions
{
    public static class MappingExtensions
    {
        public static IServiceCollection AddMappingConfiguration(this IServiceCollection services)
        {
            var config = new TypeAdapterConfig();
            config.Scan(typeof(Application.Mapping.MapConfig).Assembly);
            config.Scan(typeof(Web.Mapping.ViewModelMappingConfig).Assembly);
            services.AddSingleton(config);
            return services;
        }
    }
}
