using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using qlkh.Helpers;

namespace qlkh.Filters
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!AuthHelper.IsAdmin(context.HttpContext))
            {
                context.Result = new RedirectToActionResult("Login", "Account", new { area = "" });
            }
            base.OnActionExecuting(context);
        }
    }
}
