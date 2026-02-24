namespace CouponApp.Application.Exceptions
{
    public class MerchantProfileMissingException : Exception
    {
        public MerchantProfileMissingException() : base("Merchant profile does not exist")
        {

        }
    }
}
