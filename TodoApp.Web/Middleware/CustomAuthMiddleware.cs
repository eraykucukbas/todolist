using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace TodoApp.Web.Middleware
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var accessToken = context.Request.Cookies["AccessToken"];
            if (string.IsNullOrEmpty(accessToken) && !context.Request.Path.StartsWithSegments("/Auth"))
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // if (ex is UnauthorizedAccessException || ex is SecurityTokenException)
                // {
                    context.Response.Redirect("/Auth/Login");
                    return;
                // }
                // throw;
            }
        }
    }
}