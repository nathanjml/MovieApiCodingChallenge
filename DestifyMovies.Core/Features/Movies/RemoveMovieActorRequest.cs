using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Movies;

public class RemoveMovieActorRequest : IRequest
{
    public long MovieId { get; set; }
    public long ActorId { get; set; }
}

public class RemoveMovieActorRequestHandler : IRequestHandler<RemoveMovieActorRequest, NoResult>
{
    private readonly DbContext _dbContext;

    public RemoveMovieActorRequestHandler(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<NoResult>> HandleAsync(RemoveMovieActorRequest request, CancellationToken token = default)
    {
        var movie = await _dbContext.Set<Movie>()
            .Include(x => x.Actors)
            .FirstOrDefaultAsync(x => x.Id == request.MovieId, token);

        if (movie == null)
            return Response<NoResult>.NotFound();

        var actor = movie.Actors.FirstOrDefault(x => x.Id == request.ActorId);

        if (actor == null)
            return Response<NoResult>.NotFound();

        movie.Actors.Remove(actor);

        await _dbContext.SaveChangesAsync(token);

        return Response<NoResult>.Success();
    }
}