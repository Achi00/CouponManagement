using Microsoft.AspNetCore.Mvc;

namespace CouponApp.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult App()
        {
            var message = HttpContext.Items["ErrorMessage"]?.ToString();
            ViewBag.Message = message;

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
