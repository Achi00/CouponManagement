
using CouponApp.Domain.Enums;

namespace CouponApp.Application.Interfaces.Sercives.Auth
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Email { get; }
        string? Username { get; }
        bool IsAuthenticated { get; }
        UserRole? Role { get; }
    }
}
