using CouponApp.Application.Interfaces.Sercives.Offer;
using CouponApp.Web.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.Admin)]
    public class OffersController : Controller
    {
        private readonly IAdminOfferService _service;

        public OffersController(IAdminOfferService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Pending(CancellationToken ct)
        {
            var offers = await _service.GetPendingAsync(ct);

            return View(offers);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Guid id, CancellationToken ct)
        {
            await _service.ApproveAsync(id, ct);

            TempData["Success"] = "Offer approved";

            return RedirectToAction(nameof(Pending));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(Guid id, string reason, CancellationToken ct)
        {
            await _service.RejectAsync(id, reason, ct);

            TempData["Success"] = "Offer rejected";

            return RedirectToAction(nameof(Pending));
        }
    }
}
