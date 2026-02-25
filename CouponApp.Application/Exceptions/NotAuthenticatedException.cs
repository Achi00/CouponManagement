using Microsoft.AspNetCore.Http;

namespace CouponApp.Application.Exceptions
{
    public class NotAuthenticatedException : AppException
    {
        public NotAuthenticatedException()
            : base("User is not authenticated", StatusCodes.Status401Unauthorized)
        {
        }
    }
}
