namespace CouponApp.Application.DTOs.Reservations
{
    public class ReservationResponse
    {
        public Guid Id { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string OfferTitle { get; set; }
    }
}
