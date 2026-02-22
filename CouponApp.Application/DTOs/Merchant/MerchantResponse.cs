namespace CouponApp.Application.DTOs.Merchant
{
    public class MerchantResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string BusinessName { get; set; }
        public string Description { get; set; }
    }
}
