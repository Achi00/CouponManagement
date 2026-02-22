namespace CouponApp.Application.DTOs.Offers
{
    public class RejectOfferResponse
    {
        public Guid OfferId { get; set; }

        public string Reason { get; set; }
    }
}
