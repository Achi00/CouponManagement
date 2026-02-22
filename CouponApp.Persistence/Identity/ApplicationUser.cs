using Microsoft.AspNetCore.Identity;
using CouponApp.Domain.Enums;

namespace CouponApp.Persistence.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public UserRole Role { get; set; }
        public bool IsBlocked { get; set; }
    }
}
