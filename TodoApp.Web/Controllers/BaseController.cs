using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TodoApp.Web.Controllers
{
    public class BaseController : Controller
    {
        protected string AccessToken { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            AccessToken = context.HttpContext.Request.Cookies["AccessToken"];
        }
    }
}