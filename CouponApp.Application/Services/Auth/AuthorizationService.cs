using CouponApp.Application.Exceptions;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Domain.Enums;

namespace CouponApp.Application.Services.Auth
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ICurrentUserService _currentUser;

        public AuthorizationService(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        public void EnsureAuthenticated()
        {
            if (!_currentUser.IsAuthenticated)
            {
                throw new NotAuthenticatedException();
            }
        }

        public void EnsureRole(UserRole role)
        {
            if (_currentUser.Role != role)
            {
                throw new ForbiddenException("User does not have the required role");
            }
        }
    }
}
