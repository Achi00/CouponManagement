using Microsoft.AspNetCore.Identity;
using CouponApp.Domain.Entity;
using CouponApp.Domain.Enums;

namespace CouponApp.Persistence.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public Guid? DomainUserId { get; set; }
    }
}
