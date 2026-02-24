using CouponApp.Application.DTOs.Merchant;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Web.Models.Merchant;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.Web.Controllers
{
    [Authorize]
    public class MerchantController : Controller
    {
        private readonly IMerchantService _merchantService;

        public MerchantController(IMerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        [Authorize(Policy = "MerchantOnly")]
        public IActionResult Index()
        {
            return View();
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
                return View(vm);

            await _merchantService.RegisterAsMerchantAsync(vm.Adapt<RegisterAsMerchantRequest>(), cancellationToken);

            TempData["Success"] = "You are now a merchant.";

            return RedirectToAction(nameof(Index));
        }
    }
}
