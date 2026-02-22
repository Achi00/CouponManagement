using Microsoft.AspNetCore.Http;
using CouponApp.Application.Interfaces.Sercives.Auth;
using CouponApp.Domain.Entity;
using CouponApp.Domain.Enums;
using System.Security.Claims;

namespace CouponApp.Infrastructure.Auth
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var value = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return Guid.TryParse(value, out var guid) ? guid : null;
            }
        }

        public string? Email => User?.FindFirst(ClaimTypes.Email)?.Value;
        public string? Username => User?.FindFirst(ClaimTypes.Name)?.Value;
        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
        public UserRole? Role
        {
            get
            {
                var roleClaim = User?.FindFirst(ClaimTypes.Role)?.Value;
                return Enum.TryParse<UserRole>(roleClaim, out var role) ? role : null;
            }
        }
    }
}
