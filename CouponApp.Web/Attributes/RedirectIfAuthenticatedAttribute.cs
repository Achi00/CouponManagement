using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CouponApp.Web.Attributes
{
    // redirect user in home page if they are authenticated
    // eg: logged in user cant access /login /register
    public class RedirectIfAuthenticatedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                var returnUrl = context.HttpContext.Request.Query["ReturnUrl"].ToString();

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    context.Result = new RedirectResult(returnUrl);
                    return;
                }

                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }

}
