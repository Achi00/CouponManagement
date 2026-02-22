namespace CouponApp.Application.DTOs.Reservations
{
    public class ReservationResponse
    {
        public Guid ReservationId { get; set; }

        public DateTime ExpiresAt { get; set; }

        public int Quantity { get; set; }
        public string OfferTitle { get; set; }
    }
}
