using CouponApp.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CouponApp.API.Security
{
    public class AuthSessionService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthSessionService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task RefreshUserAsync(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            if (user != null)
            {
                await _signInManager.RefreshSignInAsync(user);
            }
        }
    }
}
