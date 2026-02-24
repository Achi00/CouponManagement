using CouponApp.Application.DTOs.Merchant;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Application.Services;
using CouponApp.Persistence.Identity;
using CouponApp.Web.Models.Merchant;
using CouponApp.Web.Security;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.Web.Controllers
{
    [Authorize]
    public class MerchantController : Controller
    {
        private readonly IMerchantService _merchantService;
        private readonly IOfferService _offerService;
        private readonly AuthSessionService _authSession;

        public MerchantController(IMerchantService merchantService, IOfferService offerService, AuthSessionService authSession)
        {
            _merchantService = merchantService;
            _offerService = offerService;
            _authSession = authSession;
        }

        [Authorize(Policy = "MerchantOnly")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var offers = await _offerService.GetByMerchantsAsync(cancellationToken);

            var vm = offers.Adapt<List<MerchantOfferViewModel>>();

            return View(vm);
        }

        public async Task<IActionResult> CreateProfile(CancellationToken cancellationToken = default)
        {
            if (await _merchantService.MerchantProfileExistsAsync(cancellationToken))
            {
                return RedirectToAction(nameof(Index));
            }

            return View(new CreateMerchantProfileViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProfile(CreateMerchantProfileViewModel vm, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await _merchantService.RegisterAsMerchantAsync(vm.Adapt<RegisterAsMerchantRequest>(), cancellationToken);

            await _authSession.RefreshUserAsync(User);

            TempData["Success"] = "You are now a merchant.";

            return RedirectToAction(nameof(Index));
        }
    }
}
