using CouponApp.Application.DTOs.Reservations;
using CouponApp.Application.Interfaces.Sercives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly ICouponService _couponService;

        public ReservationsController(
            IReservationService reservationService,
            ICouponService couponService)
        {
            _reservationService = reservationService;
            _couponService = couponService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateReservationRequest request, CancellationToken ct)
        {
            await _reservationService.CreateAsync(request.OfferId, ct);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("my")]
        public async Task<IActionResult> My(CancellationToken ct)
        {
            var reservations = await _reservationService.GetActiveByUserAsync(ct);

            return Ok(reservations);
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> Cancel(CancelReservationRequest request, CancellationToken ct)
        {
            await _reservationService.CancelAsync(request.ReservationId, ct);

            return NoContent();
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> Purchase(PurchaseReservationRequest request, CancellationToken ct)
        {
            await _couponService.PurchaseFromReservationAsync(request.ReservationId, ct);

            return Ok();
        }
    }
}
