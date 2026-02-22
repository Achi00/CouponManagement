namespace CouponApp.Application.DTOs.Merchant
{
    public class MerchantSaleResponse
    {
        public string Username { get; set; }

        public string CouponCode { get; set; }

        public DateTime PurchasedAt { get; set; }

        public string OfferTitle { get; set; }
    }
}
