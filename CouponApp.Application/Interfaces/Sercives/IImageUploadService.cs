using Microsoft.AspNetCore.Http;

namespace CouponApp.Application.Interfaces.Sercives
{
    public interface IImageUploadService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<bool> DeleteImageAsync(string publicId);
    }
}
