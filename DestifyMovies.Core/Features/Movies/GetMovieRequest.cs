using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Features.Movies.Dtos;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Movies;

public class GetMovieRequest : IRequest<MovieGetDto>
{
    public long Id { get; set; }
}

public class GetMovieRequestHandler : IRequestHandler<GetMovieRequest, MovieGetDto>
{
    private readonly DbContext _dbContext;
    private readonly IMapper _mapper;

    public GetMovieRequestHandler(DbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<Response<MovieGetDto>> HandleAsync(GetMovieRequest request, CancellationToken token = default)
    {
        var movie = await _dbContext.Set<Movie>()
            .AsNoTracking()
            .Include(x => x.Actors)
            .FirstOrDefaultAsync(x => x.Id == request.Id, token);

        return movie == null ? Response<MovieGetDto>.NotFound() : _mapper.Map<MovieGetDto>(movie).ToResponse();
    }
}