using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Identity;
using DestifyMovies.Core.Services.Authorization;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Users;

public class RegenerateUserTokenRequest : IRequest<string>
{
}

public class RegenerateUserTokenRequestHandler : IRequestHandler<RegenerateUserTokenRequest, string>
{
    private readonly DbContext _dbContext;
    private readonly IApiKeyService _apiKeyService;

    public RegenerateUserTokenRequestHandler(DbContext dbContext, IApiKeyService apiKeyService)
    {
        _dbContext = dbContext;
        _apiKeyService = apiKeyService;
    }

    public async Task<Response<string>> HandleAsync(RegenerateUserTokenRequest request, CancellationToken token = default)
    {
        var identityUser = _apiKeyService?.IdentityContext?.User?.Id ?? 0;

        var user = await _dbContext.Set<User>()
            .FirstOrDefaultAsync(x => x.Id == identityUser, token);

        //we shouldn't hit if we made it past auth scheme
        if (user == null)
            return Response<string>.NotFound();

        var apiToken = _apiKeyService.GenerateKey();

        user.UserApiToken = apiToken.AppToken;
        await _dbContext.SaveChangesAsync(token);

        return apiToken.UserToken.ToResponse();
    }
}