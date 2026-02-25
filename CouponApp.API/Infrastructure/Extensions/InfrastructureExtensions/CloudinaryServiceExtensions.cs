using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Infrastructure.ImageService;

namespace CouponApp.APi.Infrastructure.Extensions.InfrastructureExtensions
{
    public static class CloudinaryServiceExtensions
    {
        public static IServiceCollection AddCloudinaryServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<CloudinarySettings>(
                configuration.GetSection("Cloudinary"));

            services.AddScoped<IImageUploadService, CloudinaryImageUploadService>();

            return services;
        }
    }
}
