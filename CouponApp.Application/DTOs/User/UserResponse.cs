using CouponApp.Domain.Enums;

namespace CouponApp.Application.DTOs.User
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public UserRole Role { get; set; }
        public bool IsBlocked { get; set; }
    }
}
