using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Movies
{
    public class DeleteMovieRequest : IRequest
    {
        public long Id { get; set; }
    }

    public class DeleteMovieRequestHandler : IRequestHandler<DeleteMovieRequest, NoResult>
    {
        private readonly DbContext _dbContext;
        private readonly IMapper _mapper;

        public DeleteMovieRequestHandler(DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Response<NoResult>> HandleAsync(DeleteMovieRequest request, CancellationToken token = default)
        {
            var movie = await _dbContext.Set<Movie>()
                .FirstOrDefaultAsync(x => x.Id == request.Id, token);

            if (movie == null)
                return Response.NotFound();

            _dbContext.Remove(movie);
            await _dbContext.SaveChangesAsync(token);

            return Response.Success();
        }
    }
}
