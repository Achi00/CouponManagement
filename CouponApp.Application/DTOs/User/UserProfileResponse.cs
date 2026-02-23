using CouponApp.Application.DTOs.Merchant;
using CouponApp.Domain.Enums;

namespace CouponApp.Application.DTOs.User
{
    public class UserProfileResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public UserRole Role { get; set; }
        public bool IsBlocked { get; set; }
        public MerchantProfileResponse? Merchant { get; init; }
        public UserStats Stats { get; init; }
    }
}
