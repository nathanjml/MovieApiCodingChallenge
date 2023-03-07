using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Features.Actors.Dtos;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Movies
{
    public class GetMovieActorsRequest : IRequest<List<ActorGetDto>>
    {
        public long Id { get; set; }
    }

    public class GetMovieActorsRequestHandler : IRequestHandler<GetMovieActorsRequest, List<ActorGetDto>>
    {
        private readonly DbContext _dbContext;
        private readonly IMapper _mapper;

        public GetMovieActorsRequestHandler(DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Response<List<ActorGetDto>>> HandleAsync(GetMovieActorsRequest request, CancellationToken token = default)
        {
            var movie = await _dbContext.Set<Movie>()
                .AsNoTracking()
                .Include(x => x.Actors)
                .FirstOrDefaultAsync(x => x.Id == request.Id, token);

            return movie == null 
                ? Response<List<ActorGetDto>>.NotFound() 
                : movie.Actors.Select(_mapper.Map<ActorGetDto>).ToList().ToResponse();
        }
    }
}
