using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Domain.Enums;

namespace CouponApp.Application.Helpers
{
    public static class AuthHelper
    {
        public static void EnsureAuthenticated(ICurrentUserService currentUser)
        {
            if (!currentUser.IsAuthenticated)
            {
                throw new NotAuthenticatedException("User is not authenticated");
            }
        }

        public static void EnsureRole(UserRole role, ICurrentUserService currentUser)
        {
            if (currentUser.Role != role)
            {
                throw new ForbiddenException($"User does not have the required role");
            }
        }
    }
}
