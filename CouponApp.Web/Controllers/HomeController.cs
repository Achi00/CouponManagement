using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Application.Interfaces.Sercives.Offer;
using CouponApp.Web.Models.Home;
using CouponApp.Web.Models.Offer;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOfferQueryService _offerService;
        private readonly ICategoryService _categoryService;

        public HomeController(IOfferQueryService offerService, ICategoryService categoryService)
        {
            _offerService = offerService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var offers = await _offerService.GetApprovedAsync(cancellationToken);
            var categories = await _categoryService.GetAllAsync(cancellationToken);

            var offerModel = offers.Adapt<List<OfferCardViewModel>>();

            var model = new HomeViewModel
            {
                Offers = offerModel,
                Categories = categories
            };

            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        {
            var offer = await _offerService.GetDetailsAsync(id, cancellationToken);
            return View(offer);
        }
    }
}
