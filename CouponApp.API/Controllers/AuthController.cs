using CouponApp.Application.Interfaces.JWT;
using CouponApp.Persistence.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtTokenGenerator _jwt;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwt)
        {
            _userManager = userManager;
            _jwt = jwt;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Unauthorized();

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
                return Unauthorized();

            if (user.IsBlocked)
                return Forbid();

            var roles = new[] { user.Role.ToString() };

            var token = _jwt.GenerateToken(user.Id, user.Email!, roles);

            return Ok(new
            {
                access_token = token
            });
        }
    }
}
