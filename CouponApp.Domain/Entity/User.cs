using CouponApp.Domain.Enums;

namespace CouponApp.Domain.Entity
{
    public class User
    {
        public Guid Id { get; private set; }

        public string Username { get; private set; }

        public UserRole Role { get; private set; }

        public bool IsBlocked { get; private set; }

        public Merchant? Merchant { get; private set; }

        private readonly List<Coupon> _coupons = new();
        public IReadOnlyCollection<Coupon> Coupons => _coupons;

        private readonly List<Reservation> _reservations = new();
        public IReadOnlyCollection<Reservation> Reservations => _reservations;

        private User() { }

        public User(Guid id, string username, UserRole role)
        {
            Id = id;
            Username = username;
            Role = role;
            IsBlocked = false;
        }

        public void Block() => IsBlocked = true;
        public void Unblock() => IsBlocked = false;
        public bool IsAdmin() => Role == UserRole.Admin;
    }

}
