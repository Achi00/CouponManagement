using Microsoft.AspNetCore.Http;

namespace CouponApp.Application.Exceptions
{
    public class MerchantProfileMissingException : AppException
    {
        public MerchantProfileMissingException()
            : base("Merchant profile required", StatusCodes.Status403Forbidden)
        {
        }
    }
}
