using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class CookieSetFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        filterContext.HttpContext.Session.SetString(string.Empty, string.Empty);
    }
}
