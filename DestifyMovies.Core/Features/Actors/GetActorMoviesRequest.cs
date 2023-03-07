using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Features.Movies.Dtos;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Actors;

public class GetActorMoviesRequest : IRequest<List<ListMovieDto>>
{
    public long Id { get; set; }
}

public class GetActorMoviesRequestHandler : IRequestHandler<GetActorMoviesRequest, List<ListMovieDto>>
{
    private readonly DbContext _dbContext;
    private readonly IMapper _mapper;

    public GetActorMoviesRequestHandler(DbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Response<List<ListMovieDto>>> HandleAsync(GetActorMoviesRequest request, CancellationToken token = default)
    {
        var actor = await _dbContext.Set<Actor>()
            .Include(x => x.Movies)
            .FirstOrDefaultAsync(x => x.Id == request.Id, token);

        return actor == null 
            ? Response<List<ListMovieDto>>.NotFound() 
            : actor.Movies.Select(_mapper.Map<ListMovieDto>).ToList().ToResponse();
    }
}