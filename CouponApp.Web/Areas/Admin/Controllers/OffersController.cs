using CouponApp.Application.Interfaces.Sercives.Offer;
using CouponApp.Web.Areas.Admin.ViewModels;
using CouponApp.Web.Constants;
using Mapster;
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

        public async Task<IActionResult> Pending(CancellationToken cancellationToken = default)
        {
            var offers = await _service.GetPendingAsync(cancellationToken);

            var vm = offers.Adapt<List<AdminOfferViewModel>>();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(Guid id, CancellationToken cancellationToken = default)
        {
            await _service.ApproveAsync(id, cancellationToken);

            TempData["Success"] = "Offer approved";

            return RedirectToAction(nameof(Pending));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(Guid id, string reason, CancellationToken cancellationToken = default)
        {
            await _service.RejectAsync(id, reason, cancellationToken);

            TempData["Success"] = "Offer rejected";

            return RedirectToAction(nameof(Pending));
        }
    }
}
