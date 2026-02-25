using Microsoft.AspNetCore.Http;

namespace CouponApp.Application.Interfaces.Logger
{
    public interface IErrorLogger
    {
        Task LogAsync(
            Exception exception,
            HttpContext context,
            int statusCode);
    }
}
