using AutoMapper;
using DestifyMovies.Core.Features.Users.Dtos;
using DestifyMovies.Core.Identity;
using DestifyMovies.Core.Services.Authorization;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Users
{
    public class CreateUserRequest : UserDto, IRequest<string> 
    {
    }

    public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, string>
    {
        private readonly DbContext _dbContext;
        private readonly IApiKeyService _keyService;
        private readonly IMapper _mapper;

        public CreateUserRequestHandler(DbContext dbContext, IApiKeyService keyService, IMapper mapper)
        {
            _dbContext = dbContext;
            _keyService = keyService;
            _mapper = mapper;
        }

        public async Task<Response<string>> HandleAsync(CreateUserRequest request, CancellationToken token = default)
        {
            var entity = _mapper.Map<User>(request);

            var apiKey = _keyService.GenerateKey();

            entity.UserApiToken = apiKey.AppToken;

            _dbContext.Set<User>()
                .Add(entity);

            await _dbContext.SaveChangesAsync(token);

            return Response<string>.Created(apiKey.UserToken);
        }
    }
}
