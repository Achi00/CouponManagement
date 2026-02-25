using Microsoft.AspNetCore.Http;

namespace CouponApp.Application.Exceptions
{
    public class BusinessException : AppException
    {
        public BusinessException(string message)
            : base(message, StatusCodes.Status400BadRequest)
        {
        }
    }
}
