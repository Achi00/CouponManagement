namespace CouponApp.Application.DTOs.Reservations
{
    public class ReserveOfferRequest
    {
        public Guid OfferId { get; set; }

        public int Quantity { get; set; }
    }
}
