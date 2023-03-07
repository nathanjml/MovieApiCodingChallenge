using System.Net;
using DestifyMovies.Core.Services.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace DesitfyMovies.Filters;

public class RateLimiterFilter : IActionFilter
{
    private readonly IApiKeyService _apiKeyService;
    private readonly IDistributedCache _cache;
    private const int MaxSeconds = 1;
    private readonly int _anonymousLimit;
    private readonly int _userLimit;

    public RateLimiterFilter(IApiKeyService apiKeyService, IDistributedCache cache, IConfiguration configuration)
    {
        _apiKeyService = apiKeyService;
        _cache = cache;
        _anonymousLimit = int.Parse(configuration.GetSection("RateLimiting")["AnonymousLimitPerSecond"]!);
        _userLimit = int.Parse(configuration.GetSection("RateLimiting")["UserLimitPerSecond"]!);
    }
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var isAnonymous = _apiKeyService.IdentityContext?.User == null;
        var httpContext = context.HttpContext;

        // as per the coding challenge doc, we only want to rate limit on GET endpoints
        var isGetEndpoint = context.ActionDescriptor.EndpointMetadata.Any(x => x is HttpGetAttribute);
        if (!isGetEndpoint) return;

        // if api key is present, we use that. otherwise, ip is the next best thing.
        // NOTE: this strategy is not sufficient in a load balanced env
        var key = isAnonymous
            ? $"{context.ActionDescriptor.DisplayName}_{httpContext.Connection.RemoteIpAddress}"
            : $"{context.ActionDescriptor.DisplayName}_{_apiKeyService.IdentityContext!.User!.Id}";

        var limit = isAnonymous ? _anonymousLimit : _userLimit;

        var clientRateUsage = GetClientRateUsage(key);
        if (clientRateUsage != null &&
            DateTime.UtcNow < (clientRateUsage.LastSuccessfulResponseTime.AddSeconds(MaxSeconds)) &&
            clientRateUsage.RequestsCompletedSuccessfully >= limit)
        {
            // setting result short-circuits the pipeline
            context.Result = new StatusCodeResult((int)HttpStatusCode.TooManyRequests);
            return;
        }

        UpdateClientRateUsage(key, clientRateUsage);
    }

    public void OnActionExecuted(ActionExecutedContext context) { }

    private ClientRateUsage? GetClientRateUsage(string key)
    {
        var bytes = _cache.Get(key);
        if(bytes == null) return null;

        using var sr = new StreamReader(new MemoryStream(bytes));
        return JsonConvert.DeserializeObject<ClientRateUsage>(sr.ReadToEnd());
    }

    private void UpdateClientRateUsage(string key, ClientRateUsage? clientRateUsage)
    {

        var clientTime = clientRateUsage?.LastSuccessfulResponseTime ?? DateTime.UtcNow;
        var hasTimeElapsed = DateTime.UtcNow > clientTime.AddSeconds(MaxSeconds);

        var bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new ClientRateUsage
        {
            LastSuccessfulResponseTime = hasTimeElapsed ? DateTime.UtcNow : clientTime,
            RequestsCompletedSuccessfully = hasTimeElapsed ? 1 : (clientRateUsage?.RequestsCompletedSuccessfully ?? 0) + 1,
        });

        _cache.Set(key, bytes);
    }
}

[Serializable]
internal class ClientRateUsage
{
    public DateTime LastSuccessfulResponseTime { get; set; }
    public int RequestsCompletedSuccessfully { get; set; }
}