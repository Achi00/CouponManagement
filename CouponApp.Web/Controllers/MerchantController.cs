using CouponApp.Application.DTOs.Merchant;
using CouponApp.Application.DTOs.Offers;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Web.Factories;
using CouponApp.Web.Models.Merchant;
using CouponApp.Web.Models.Offer;
using CouponApp.Web.Security;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.Web.Controllers
{
    [Authorize]
    public class MerchantController : Controller
    {
        private readonly IMerchantService _merchantService;
        private readonly IOfferService _offerService;
        private readonly IImageUploadService _imageUploadService;
        private readonly AuthSessionService _authSession;
        private readonly CreateOfferViewModelFactory _factory;
        private readonly TypeAdapterConfig _mapConfig;

        public MerchantController(
            IMerchantService merchantService, 
            IOfferService offerService,
            IImageUploadService imageUploadService,
            AuthSessionService authSession,
            TypeAdapterConfig mapConfig,
            CreateOfferViewModelFactory factory)
        {
            _merchantService = merchantService;
            _offerService = offerService;
            _imageUploadService = imageUploadService;
            _authSession = authSession;
            _mapConfig = mapConfig;
            _factory = factory;
        }

        [Authorize(Policy = "MerchantOnly")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var offers = await _offerService.GetByMerchantsAsync(cancellationToken);

            var vm = offers.Adapt<List<MerchantOfferViewModel>>();

            return View(vm);
        }

        // register as merchant as customer
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

        // create offers as merchant
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var vm = await _factory.CreateAsync(cancellationToken);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOfferViewModel vm, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                // repopulate if invalid modelstate
                var freshVm = await _factory.CreateAsync(cancellationToken);
                vm.Categories = freshVm.Categories;
                return View(vm);
            }
            // upload image and attach URL before mapping
            var imageUrl = await _imageUploadService.UploadImageAsync(vm.ImageFile);

            vm.ImageUrl = imageUrl;
            Console.WriteLine(imageUrl);

            await _offerService.CreateAsync(vm.Adapt<CreateOfferRequest>(), cancellationToken);

            TempData["Success"] = "Offer submitted for approval.";

            return RedirectToAction(nameof(Index));
        }

        // edit merchant offer
        public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken = default)
        {
            var offer = await _offerService.GetDetailsAsync(id, cancellationToken);
            var vm = offer.Adapt<EditOfferViewModel>();
            vm.Categories = (await _factory.CreateAsync(cancellationToken)).Categories;
            vm.ExistingImageUrl = offer.ImageUrl;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, EditOfferViewModel vm, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                var fresh = await _factory.CreateAsync(cancellationToken);
                vm.Categories = fresh.Categories;
                return View(vm);
            }

            if (vm.NewImageFile != null && vm.NewImageFile.Length > 0)
            {
                vm.ExistingImageUrl = await _imageUploadService.UploadImageAsync(vm.NewImageFile);
            }

            Console.WriteLine($"ExistingImageUrl before mapping: {vm.ExistingImageUrl}");

            var dto = vm.Adapt<UpdateOfferRequest>(_mapConfig);

            Console.WriteLine($"dto.ImageUrl after mapping: {dto.ImageUrl}");


            await _offerService.UpdateAsync(id, dto, cancellationToken);

            TempData["Success"] = "Offer updated";

            return RedirectToAction(nameof(Index));
        }

        // delete offer
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            await _offerService.DeleteAsync(id, cancellationToken);

            TempData["Success"] = "Offer deleted";

            return RedirectToAction(nameof(Index));
        }
    }
}
