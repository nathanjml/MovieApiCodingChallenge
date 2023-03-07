using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.MovieRatings;

public class DeleteMovieRatingRequest : IRequest
{
    public long RatingId { get; set; }
    public long MovieId { get; set; }
}

public class DeleteMovieRatingRequestHandler : IRequestHandler<DeleteMovieRatingRequest, NoResult>
{
    private readonly DbContext _dbContext;
    private readonly IMapper _mapper;

    public DeleteMovieRatingRequestHandler(DbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Response<NoResult>> HandleAsync(DeleteMovieRatingRequest request, CancellationToken token = default)
    {
        var rating = await _dbContext.Set<MovieRating>()
            .AsNoTracking()
            .Include(x => x.Movie)
            .FirstOrDefaultAsync(x => x.Id == request.RatingId && x.MovieId == request.MovieId, token);

        if (rating == null)
            return Response<NoResult>.NotFound();

        _dbContext.Set<MovieRating>().Remove(rating);

        await _dbContext.SaveChangesAsync(token);

        return Response<NoResult>.Success();
    }
}