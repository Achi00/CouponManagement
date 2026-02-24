using Microsoft.AspNetCore.Mvc;

namespace CouponApp.Web.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
