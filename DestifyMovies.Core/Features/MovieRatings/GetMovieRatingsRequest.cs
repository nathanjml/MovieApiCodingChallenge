using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Features.MovieRatings.Dtos;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.MovieRatings;

public class GetMovieRatingsRequest : IRequest<List<ListMovieRatingDto>>
{
    public long MovieId { get; set; }
}

public class GetMovieRatingsRequestHandling : IRequestHandler<GetMovieRatingsRequest, List<ListMovieRatingDto>>
{
    private readonly DbContext _dbContext;
    private readonly IMapper _mapper;

    public GetMovieRatingsRequestHandling(DbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public Task<Response<List<ListMovieRatingDto>>> HandleAsync(GetMovieRatingsRequest request, CancellationToken token = default)
    {
        var movieRatings = _dbContext.Set<MovieRating>()
            .AsNoTracking()
            .Include(x => x.Movie)
            .Where(x => x.MovieId == request.MovieId)
            .AsEnumerable()
            .Select(_mapper.Map<ListMovieRatingDto>)
            .ToList();

        return movieRatings.ToResponseAsync();
    }
}