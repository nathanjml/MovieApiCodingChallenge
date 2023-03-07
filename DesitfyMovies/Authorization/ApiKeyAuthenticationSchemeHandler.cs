using System.Security.Claims;
using System.Text.Encodings.Web;
using DestifyMovies.Core.Services.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DesitfyMovies.Authorization
{
    public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
    }
    public class ApiKeyAuthenticationSchemeHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
    {
        private readonly IApiKeyService _apiKeyService;

        public ApiKeyAuthenticationSchemeHandler(IApiKeyService apiKeyService, IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _apiKeyService = apiKeyService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var apiKey = Context.Request.Headers["X-API-KEY"].FirstOrDefault();
            if (apiKey == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Token not present"));
            }
            var valid = _apiKeyService.ValidateKey(apiKey);
            if (valid == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Token"));
            }

            var claims = new[] { new Claim(ClaimTypes.Email, valid.EmailAddress) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
