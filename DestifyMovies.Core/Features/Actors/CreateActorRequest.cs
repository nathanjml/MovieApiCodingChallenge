using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Features.Actors.Dtos;
using DestifyMovies.Core.Services.Mediator;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Actors
{
    public class CreateActorRequest : ActorDto, IRequest<ActorGetDto>
    {
    }

    public class CreateActorRequestValidator : AbstractValidator<CreateActorRequest>
    {
        public CreateActorRequestValidator()
        {
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.CountryOfOrigin).NotEmpty();
        }
    }

    public class CreateActorRequestHandler : IRequestHandler<CreateActorRequest, ActorGetDto>
    {
        private readonly DbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateActorRequestHandler(DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Response<ActorGetDto>> HandleAsync(CreateActorRequest request, CancellationToken token = default)
        {
            var entity = _mapper.Map<Actor>(request);
            _dbContext.Set<Actor>().Add(entity);

            await _dbContext.SaveChangesAsync(token);
            
            return Response<ActorGetDto>.Created(_mapper.Map<ActorGetDto>(entity));
        }
    }
}
