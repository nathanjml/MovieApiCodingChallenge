using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Identity;
using DestifyMovies.Core.Services.Authorization;
using DestifyMovies.Core.Services.Mediator;

namespace DestifyMovies.Core.Features.ApiTokens
{
    //TODO: Remove
    public class CreateApiTokenRequest : IRequest<string>
    {
    }

    public class CreateApiTokenRequestHandler : IRequestHandler<CreateApiTokenRequest, string>
    {
        private readonly DataContext _dataContext;
        private readonly IApiKeyService _apiKeyService;

        public CreateApiTokenRequestHandler(DataContext dataContext, IApiKeyService apiKeyService)
        {
            _dataContext = dataContext;
            _apiKeyService = apiKeyService;
        }
        public async Task<Response<string>> HandleAsync(CreateApiTokenRequest request, CancellationToken token = default)
        {
            var tokenResponse = _apiKeyService.GenerateKey();
            var identityUser = _apiKeyService.IdentityContext?.User;

            //the user already has an apiToken generated; therefore, we should regen the token
            if (identityUser != null)
            {
                var userEntity = _dataContext.Set<User>()
                    .First(x => x.Id == identityUser.Id);

                userEntity.UserApiToken = tokenResponse.AppToken;

                await _dataContext.SaveChangesAsync(token);
            }

            return new Response<string>{Result = tokenResponse.UserToken};
        }
    }
}
