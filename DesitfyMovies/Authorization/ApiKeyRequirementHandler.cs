using DestifyMovies.Core.Services.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace DesitfyMovies.Authorization;

public class ApiKeyRequirementHandler : AuthorizationHandler<ApiKeyRequirement>
{
    private readonly IApiKeyService _apiKeyService;

    public ApiKeyRequirementHandler(IApiKeyService apiKeyService)
    {
        _apiKeyService = apiKeyService;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyRequirement requirement)
    {
        if (context.Resource is not HttpContext httpContext) return Task.CompletedTask;

        var apiToken = httpContext.Request.Headers["X-API-KEY"].FirstOrDefault();
        if(apiToken != null && _apiKeyService.ValidateKey(apiToken) != null)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}