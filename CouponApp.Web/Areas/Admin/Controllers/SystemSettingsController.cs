using CouponApp.Application.DTOs.Settings;
using CouponApp.Application.Interfaces.Sercives;
using CouponApp.Web.Areas.Admin.ViewModels;
using CouponApp.Web.Constants;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CouponApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppRoles.Admin)]
    public class SystemSettingsController : Controller
    {
        private readonly ISystemSettingsService _service;

        public SystemSettingsController(ISystemSettingsService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var settings = await _service.GetAsync(cancellationToken);

            var vm = settings.Adapt<SystemSettingsViewModel>();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SystemSettingsViewModel vm, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await _service.UpdateAsync(vm.Adapt<UpdateSystemSettingsRequest>(), cancellationToken);

            TempData["Success"] = "Settings updated";

            return RedirectToAction(nameof(Index));
        }
    }
}
