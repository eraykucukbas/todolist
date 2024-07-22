using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace TodoApp.Web.Middleware
{
    public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public CustomAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var accessToken = Context.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(accessToken))
            {
                return AuthenticateResult.Fail("No Access Token");
            }

            var claims = new[] { new Claim(ClaimTypes.Name, "User") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}