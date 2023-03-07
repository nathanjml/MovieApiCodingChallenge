using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Features.MovieRatings.Dtos;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.MovieRatings;

public class UpdateMovieRatingsRequest : IRequest<MovieRatingGetDto>
{
    public UpdateMovieRatingsRequest(long movieId, long ratingId, MovieRatingDto dto)
    {
        MovieRatingDto = dto;
        MovieId = movieId;
        RatingId = ratingId;
    }
    public long MovieId { get; set; }
    public long RatingId { get; set; }

    public MovieRatingDto MovieRatingDto { get; set; } = new();
}

public class UpdateMovieRatingsRequestHandler : IRequestHandler<UpdateMovieRatingsRequest, MovieRatingGetDto>
{
    private readonly DbContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateMovieRatingsRequestHandler(DbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Response<MovieRatingGetDto>> HandleAsync(UpdateMovieRatingsRequest request, CancellationToken token = default)
    {
        var movieRating = await _dbContext.Set<MovieRating>()
            .Include(x => x.Movie)
            .FirstOrDefaultAsync(x => x.Id == request.RatingId && x.MovieId == request.MovieId, token);

        if (movieRating == null)
            return Response<MovieRatingGetDto>.NotFound();

        _mapper.Map(request.MovieRatingDto, movieRating);

        await _dbContext.SaveChangesAsync(token);

        return _mapper.Map<MovieRatingGetDto>(movieRating).ToResponse();
    }
}