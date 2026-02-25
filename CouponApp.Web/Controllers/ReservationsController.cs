using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Domain.Entity;
using CouponApp.Web.Models.Reservation;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.Web.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly ICouponService _couponService;

        public ReservationsController(IReservationService reservationService, ICouponService couponService)
        {
            _reservationService = reservationService;
            _couponService = couponService;
        }

        // make reservation
        [HttpPost]
        public async Task<IActionResult> Create(Guid offerId, CancellationToken ct = default)
        {
            await _reservationService.CreateAsync(offerId, ct);

            return RedirectToAction(nameof(My));
        }

        public async Task<IActionResult> My(CancellationToken ct = default)
        {
            var reservations = await _reservationService.GetActiveByUserAsync(ct);

            var vm = reservations.Adapt<List<ReservationViewModel>>();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken ct = default)
        {
            await _reservationService.CancelAsync(id, ct);

            return RedirectToAction(nameof(My));
        }

        [HttpPost]
        public async Task<IActionResult> PurchaseFromReservation(Guid reservationId, CancellationToken ct)
        {
            await _couponService.PurchaseFromReservationAsync(reservationId, ct);

            return RedirectToAction(nameof(My));
        }
    }
}
