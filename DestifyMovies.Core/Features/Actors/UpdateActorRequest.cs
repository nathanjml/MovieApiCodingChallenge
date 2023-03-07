using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Features.Actors.Dtos;
using DestifyMovies.Core.Services.Mediator;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Actors
{
    public class UpdateActorRequest : IRequest<ActorGetDto>
    {
        public UpdateActorRequest(long id, ActorEditDto actor)
        {
            Actor = actor;
            Id = id;
        }
        public long Id { get; }
        public ActorEditDto Actor { get; }
    }

    public class UpdateActorRequestValidator : AbstractValidator<UpdateActorRequest>
    {
        public UpdateActorRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Actor.LastName).NotEmpty();
            RuleFor(x => x.Actor.FirstName).NotEmpty();
        }
    }

    public class UpdateActorRequestHandler : IRequestHandler<UpdateActorRequest, ActorGetDto>
    {
        private readonly DbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateActorRequestHandler(DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Response<ActorGetDto>> HandleAsync(UpdateActorRequest request, CancellationToken token = default)
        {
            var actor = await _dbContext.Set<Actor>()
                .FirstOrDefaultAsync(x => x.Id == request.Id, token);

            if (actor == null)
                return Response<ActorGetDto>.NotFound();

            _mapper.Map(request.Actor, actor);

            await _dbContext.SaveChangesAsync(token);

            return _mapper.Map<ActorGetDto>(actor).ToResponse();
        }
    }
}
