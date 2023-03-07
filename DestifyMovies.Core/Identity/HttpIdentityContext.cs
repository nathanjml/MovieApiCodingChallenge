using DestifyMovies.Core.Services.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Identity;

public class HttpIdentityContext : IIdentityContext
{
    private User? _cachedUser;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private IApiKeyService? _apiKeyService;

    public HttpIdentityContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IApiKeyService ApiKeyService
    {
        get => _apiKeyService;
        set
        {
            if (_apiKeyService != null && !Equals(_apiKeyService, value))
                InvalidateCache();

            _apiKeyService = value;
        }
    }

    public User? User
    {
        get
        {
            if (_cachedUser != null) return _cachedUser;
            var apiToken = TryGetApiToken();

            if (apiToken == null) return _cachedUser = null;

            var user = _apiKeyService.ValidateKey(apiToken);

            return _cachedUser = user;
        }
    }

    private string? TryGetApiToken()
    {
        var foundToken = _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("x-api-key", out var token);

        if (foundToken) return token;
        return null;
    }

    private void InvalidateCache()
    {
        _cachedUser = null;
    }
}

public class HttpIdentityContextFactory : IIdentityContextFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpIdentityContextFactory(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IIdentityContext Create(IApiKeyService apiKeyService)
    {
       return new HttpIdentityContext(_httpContextAccessor)
       {
           ApiKeyService = apiKeyService
       };
    }
}