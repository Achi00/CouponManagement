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
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var categories = await _service.GetAllAsync(ct);

            var vm = categories.Adapt<List<CategoryViewModel>>();

            return View(vm);
        }
    }
}
