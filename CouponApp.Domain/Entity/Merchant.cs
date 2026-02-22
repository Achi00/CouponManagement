namespace CouponApp.Domain.Entity
{
    public class Merchant
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string BusinessName { get; set; }
        public string Description { get; set; }

        public User User { get; set; }
        public ICollection<Offer> Offers { get; set; }
    }
}
