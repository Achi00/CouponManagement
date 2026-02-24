using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Web.Models.Reservation;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.Web.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly IReservationService _service;

        public ReservationsController(IReservationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid offerId, CancellationToken ct = default)
        {
            var reservation = await _service.CreateAsync(offerId, ct);

            return RedirectToAction(nameof(My));
        }

        public async Task<IActionResult> My(CancellationToken ct = default)
        {
            var reservations = await _service.GetActiveByUserAsync(ct);

            var vm = reservations.Adapt<List<ReservationViewModel>>();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken ct = default)
        {
            await _service.CancelAsync(id, ct);

            return RedirectToAction(nameof(My));
        }
    }
}
