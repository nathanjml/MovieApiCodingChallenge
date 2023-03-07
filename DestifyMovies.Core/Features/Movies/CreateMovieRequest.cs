using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Features.Movies.Dtos;
using DestifyMovies.Core.Services.Mediator;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Movies
{
    public class CreateMovieRequest : MovieDto, IRequest<MovieGetDto>
    {
    }

    public class CreateMovieRequestValidator : AbstractValidator<CreateMovieRequest>
    {
        public CreateMovieRequestValidator(DbContext dbContext)
        {
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.ReleasedOn)
                .NotEmpty()
                .DependentRules(() =>
                {
                    RuleFor(y => y.ReleasedOn).Must(str => DateOnly.TryParse(str, out var result));
                });
            RuleForEach(x => x.ActorIds)
                .Must(x => dbContext.Set<Actor>().FirstOrDefault(y => y.Id == x) != null);
        }
    }

    public class CreateMovieRequestHandler : IRequestHandler<CreateMovieRequest, MovieGetDto>
    {
        private readonly DbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateMovieRequestHandler(DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Response<MovieGetDto>> HandleAsync(CreateMovieRequest request, CancellationToken token = default)
        {
            var actors = await _dbContext.Set<Actor>()
                .Where(x => request.ActorIds != null && request.ActorIds.Contains(x.Id))
                .ToListAsync(token);

            var movieEntity = _mapper.Map<Movie>(request);
            movieEntity.Actors = actors;

            _dbContext.Add(movieEntity);
            await _dbContext.SaveChangesAsync(token);

            return Response<MovieGetDto>.Created(_mapper.Map<MovieGetDto>(movieEntity));
        }
    }
}
