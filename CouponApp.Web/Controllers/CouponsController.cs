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
        private readonly IUserProfileService _service;

        public CouponsController(IUserProfileService service)
        {
            _service = service;
        }

        public async Task<IActionResult> My(CancellationToken ct)
        {
            var coupons = await _service.GetMyCouponsAsync(ct);

            var vm = coupons.Adapt<List<CouponViewModel>>();

            return View(vm);
        }
    }
}
