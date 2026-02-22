using CouponApp.Domain.Enums;

namespace CouponApp.Application.Interfaces.Sercives.Auth
{
    public interface IAuthorizationService
    {
        void EnsureAuthenticated();
        void EnsureRole(UserRole role);
    }
}
