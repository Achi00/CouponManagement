using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Web.Models.Home;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly ICategoryService _categoryService;

        public HomeController(IOfferService offerService, ICategoryService categoryService)
        {
            _offerService = offerService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var offers = await _offerService.GetApprovedAsync(cancellationToken);
            var categories = await _categoryService.GetAllAsync(cancellationToken);

            var model = new HomeViewModel
            {
                Offers = offers,
                Categories = categories
            };

            return View(model);
        }

        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
        {
            var offer = await _offerService.GetDetailsAsync(id, cancellationToken);
            return View(offer);
        }
    }
}
