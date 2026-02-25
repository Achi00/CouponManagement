using Microsoft.AspNetCore.Http;

namespace CouponApp.Application.Exceptions
{
    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message = "Access denied")
            : base(message, StatusCodes.Status403Forbidden)
        {
        }
    }
}
