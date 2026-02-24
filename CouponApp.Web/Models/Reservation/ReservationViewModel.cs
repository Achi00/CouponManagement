namespace CouponApp.Web.Models.Reservation
{
    public class ReservationViewModel
    {
        public Guid Id { get; set; }

        public Guid OfferId { get; set; }

        public string OfferTitle { get; set; }

        public DateTime ExpiresAt { get; set; }

        public int MinutesRemaining { get; set; }

        public decimal Price { get; set; }
    }
}
