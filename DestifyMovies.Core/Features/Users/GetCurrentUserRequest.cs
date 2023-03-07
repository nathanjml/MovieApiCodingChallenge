using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Features.Users.Dtos;
using DestifyMovies.Core.Services.Authorization;
using DestifyMovies.Core.Services.Mediator;

namespace DestifyMovies.Core.Features.Users
{
    public class GetCurrentUserRequest : IRequest<UserDto>
    {
    }

    public class GetCurrentUserRequestHandler : IRequestHandler<GetCurrentUserRequest, UserDto>
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IApiKeyService _apiKeyService;

        public GetCurrentUserRequestHandler(DataContext dataContext, IMapper mapper, IApiKeyService apiKeyService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _apiKeyService = apiKeyService;
        }
        public Task<Response<UserDto>> HandleAsync(GetCurrentUserRequest request, CancellationToken token = default)
        {
            var user = _apiKeyService.IdentityContext?.User;

            return user == null ? Response<UserDto>.NotFoundAsync() : _mapper.Map<UserDto>(user).ToResponseAsync();
        }
    }
}
