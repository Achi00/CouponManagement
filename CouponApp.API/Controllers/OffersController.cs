using CouponApp.Application.DTOs.Search;
using CouponApp.Application.Interfaces.Sercives.Offer;
using CouponApp.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OffersController : ControllerBase
    {
        private readonly IOfferQueryService _offerService;

        public OffersController(IOfferQueryService offerService)
        {
            _offerService = offerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOffers(
            [FromQuery] OfferFilterQuery filter,
            CancellationToken ct = default)
        {
            var offers = await _offerService.GetApprovedAsync(filter, ct);

            return Ok(offers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(Guid id, CancellationToken ct)
        {
            var offer = await _offerService.GetDetailsAsync(id, ct);
            if (offer == null)
            {
                return NotFound();
            }

            return Ok(offer);
        }
    }
}
