using CouponApp.Application.Interfaces.Sercives;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
        private readonly IUserProfileService _userService;

        public CouponsController(IUserProfileService userService)
        {
            _userService = userService;
        }

        [HttpGet("my")]
        public async Task<IActionResult> My(CancellationToken ct)
        {
            var coupons = await _userService.GetMyCouponsAsync(ct);
            return Ok(coupons);
        }
    }
}
