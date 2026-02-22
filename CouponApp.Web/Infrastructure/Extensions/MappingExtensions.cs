using Mapster;

namespace CouponApp.Web.Infrastructure.Extensions
{
    public static class MappingExtensions
    {
        public static IServiceCollection AddMappingConfiguration(this IServiceCollection services)
        {
            var applicationConfig = new TypeAdapterConfig();
            applicationConfig.Scan(typeof(Application.Mapping.MapConfig).Assembly);
            services.AddSingleton(applicationConfig);

            var presentationConfig = new TypeAdapterConfig();
            presentationConfig.Scan(typeof(Web.Mapping.ViewModelMappingConfig).Assembly);
            services.AddSingleton(presentationConfig);

            return services;
        }
    }
}
