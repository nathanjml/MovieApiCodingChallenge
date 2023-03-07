using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Features.Movies.Dtos;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Movies
{
    public class GetMoviesRequest : IRequest<List<ListMovieDto>>
    {
        public string? Title { get; set; }
    }

    public class GetMoviesRequestHandler : IRequestHandler<GetMoviesRequest, List<ListMovieDto>>
    {
        private readonly DbContext _dbContext;
        private readonly IMapper _mapper;

        public GetMoviesRequestHandler(DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Response<List<ListMovieDto>>> HandleAsync(GetMoviesRequest request, CancellationToken token = default)
        {
            var movies = await _dbContext.Set<Movie>()
                .AsNoTracking()
                .AsQueryable()
                .WhereIf(() => request.Title != null, x => x.Title.Contains(request.Title!))
                .ToListAsync(token);

            return movies
                .Select(_mapper.Map<ListMovieDto>)
                .ToList()
                .ToResponse();
        }
    }
}
