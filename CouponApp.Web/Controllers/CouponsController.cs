using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Web.Models.Coupon;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.Web.Controllers
{
    [Authorize]
    public class CouponsController : Controller
    {
        private readonly IUserProfileService _userService;
        private readonly ICouponService _couponService;

        public CouponsController(IUserProfileService userService, ICouponService couponService)
        {
            _userService = userService;
            _couponService = couponService;
        }

        public async Task<IActionResult> My(CancellationToken ct)
        {
            var coupons = await _userService.GetMyCouponsAsync(ct);

            var vm = coupons.Adapt<List<CouponViewModel>>();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Purchase(Guid offerId, CancellationToken ct)
        {
            await _couponService.PurchaseAsync(offerId, ct);

            return RedirectToAction(nameof(My));
        }
    }
}
