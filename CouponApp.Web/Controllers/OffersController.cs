using CouponApp.Application.DTOs.Offers;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Web.Models.Offer;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.Web.Controllers
{
    public class OffersController : Controller
    {
        private readonly IOfferService _offerService;

        public OffersController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var offers = await _offerService.GetApprovedAsync(cancellationToken);

            var vm = offers.Adapt<List<OfferListItemViewModel>>();

            return View(vm);
        }

        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken = default)
        {
            var offer = await _offerService.GetDetailsAsync(id, cancellationToken);

            var vm = offer.Adapt<OfferDetailsViewModel>();

            return View(vm);
        }
    }
}
