using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Features.Actors.Dtos;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Movies;

public class AddMovieActorRequest : IRequest<List<ActorGetDto>>
{
    public long MovieId { get; set; }
    public long ActorId { get; set; }
}

public class AddMovieActorRequestHandler : IRequestHandler<AddMovieActorRequest, List<ActorGetDto>>
{
    private readonly DbContext _dbContext;
    private readonly IMapper _mapper;

    public AddMovieActorRequestHandler(DbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Response<List<ActorGetDto>>> HandleAsync(AddMovieActorRequest request, CancellationToken token = default)
    {
        var movie = await _dbContext.Set<Movie>()
            .Include(x => x.Actors)
            .FirstOrDefaultAsync(x => x.Id == request.MovieId, token);

        if (movie == null)
            return Response<List<ActorGetDto>>.NotFound();

        var actor = await _dbContext.Set<Actor>()
            .FirstOrDefaultAsync(x => x.Id == request.ActorId, token);

        if(actor == null)
            return Response<List<ActorGetDto>>.NotFound();

        if (movie.Actors.Any(x => x.Id == actor.Id))
            return Response<List<ActorGetDto>>.BadRequest();

        movie.Actors.Add(actor);

        await _dbContext.SaveChangesAsync(token);

        return movie.Actors.Select(_mapper.Map<ActorGetDto>).ToList().ToResponse();
    }
}