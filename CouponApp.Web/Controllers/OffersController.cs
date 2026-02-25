using CouponApp.Application.DTOs.Offers;
using CouponApp.Application.DTOs.Search;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Application.Interfaces.Sercives.Offer;
using CouponApp.Application.Services;
using CouponApp.Web.Areas.Admin.ViewModels;
using CouponApp.Web.Models.Offer;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.Web.Controllers
{
    public class OffersController : Controller
    {
        private readonly IOfferQueryService _offerService;
        private readonly ICategoryService _categoryService;

        public OffersController(IOfferQueryService offerService, ICategoryService categoryService)
        {
            _offerService = offerService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(OfferFilterQuery filter, CancellationToken cancellationToken = default)
        {
            var offers = await _offerService.GetApprovedAsync(filter, cancellationToken);
            var vm = offers.Adapt<List<OfferListItemViewModel>>();

            var pageVm = new OffersIndexViewModel
            {
                Offers = vm,
                SearchQuery = filter.SearchQuery,
                SelectedCategoryId = filter.SelectedCategoryId,
                Categories = (await _categoryService.GetAllAsync(cancellationToken))
                .Adapt<List<CategoryViewModel>>()
            };

            return View(pageVm);
        }

        public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken = default)
        {
            var offer = await _offerService.GetDetailsAsync(id, cancellationToken);

            var vm = offer.Adapt<OfferDetailsViewModel>();

            return View(vm);
        }
    }
}
