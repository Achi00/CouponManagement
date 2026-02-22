namespace CouponApp.Application.DTOs.Offers
{
    public class PendingOfferResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string MerchantName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
