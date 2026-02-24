using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace CouponApp.Persistence.Identity
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public AppClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            identity.AddClaim(new Claim(
                ClaimTypes.Role,
                user.Role.ToString()));

            return identity;
        }
    }
}
