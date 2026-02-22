namespace CouponApp.Application.DTOs.Coupons
{
    public class MerchantSaleResponse
    {
        public string Username { get; set; }
        public string CouponCode { get; set; }
        public DateTime PurchasedAt { get; set; }
    }
}
