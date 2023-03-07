using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Features.Actors.Dtos;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Actors
{
    public class GetActorRequest : IRequest<ActorGetDto>
    {
        public long Id { get; set; }
    }

    public class GetActorRequestHandler : IRequestHandler<GetActorRequest, ActorGetDto>
    {
        private readonly DbContext _dbContext;
        private readonly IMapper _mapper;

        public GetActorRequestHandler(DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Response<ActorGetDto>> HandleAsync(GetActorRequest request, CancellationToken token = default)
        {
            var actor = await _dbContext.Set<Actor>()
                .FirstOrDefaultAsync(x => x.Id == request.Id, token);

            if (actor == null)
                return Response<ActorGetDto>.NotFound();

            return _mapper.Map<ActorGetDto>(actor).ToResponse();
        }
    }
}
