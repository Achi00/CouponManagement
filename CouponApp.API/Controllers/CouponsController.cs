using CouponApp.Application.DTOs.Coupons;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(UserRole.Customer))]
    public class CouponsController : ControllerBase
    {
        private readonly IUserProfileService _userService;
        private readonly ICouponService _couponService;

        public CouponsController(
            IUserProfileService userService,
            ICouponService couponService)
        {
            _userService = userService;
            _couponService = couponService;
        }

        [HttpGet("my")]
        public async Task<IActionResult> My(CancellationToken ct)
        {
            var coupons = await _userService.GetMyCouponsAsync(ct);
            return Ok(coupons);
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> Purchase(
    PurchaseCouponRequest request,
    CancellationToken ct = default)
        {
            await _couponService.PurchaseAsync(request.OfferId, ct);

            return Ok();
        }
    }
}
