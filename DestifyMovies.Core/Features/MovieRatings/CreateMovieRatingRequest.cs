using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Features.MovieRatings.Dtos;
using DestifyMovies.Core.Identity;
using DestifyMovies.Core.Services.Authorization;
using DestifyMovies.Core.Services.Mediator;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.MovieRatings
{
    public class CreateMovieRatingRequest : MovieRatingDto, IRequest<MovieRatingGetDto>
    {
        public long MovieId { get; set; }
    }

    public class CreateMovieRatingRequestValidator : AbstractValidator<CreateMovieRatingRequest>
    {
        public CreateMovieRatingRequestValidator(DbContext dbContext)
        {
            RuleFor(x => x.MovieId).NotEmpty();
            RuleFor(x => x.Rating).IsInRange(1, 10);

            RuleFor(x => dbContext.Set<Movie>()
                    .FirstOrDefault(y => y.Id == x.MovieId))
                    .NotNull();
        }
    }

    public class CreateMovieRatingRequestHandling : IRequestHandler<CreateMovieRatingRequest, MovieRatingGetDto>
    {
        private readonly DbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IIdentityContext? _identityContext;

        public CreateMovieRatingRequestHandling(DbContext dbContext, IMapper mapper, IApiKeyService apiKeyService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _identityContext = apiKeyService.IdentityContext;
        }
        public async Task<Response<MovieRatingGetDto>> HandleAsync(CreateMovieRatingRequest request, CancellationToken token = default)
        {
            var entity = _mapper.Map<MovieRating>(request);
            entity.RatingSubmittedBy = _identityContext?.User?.EmailAddress ?? "SYSTEM";
            entity.MovieId = request.MovieId;

            _dbContext.Set<MovieRating>()
                .Add(entity);

            await _dbContext.SaveChangesAsync(token);

            return Response<MovieRatingGetDto>.Created(_mapper.Map<MovieRatingGetDto>(entity));
        }
    }
}
