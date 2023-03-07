using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Features.MovieRatings.Dtos;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.MovieRatings
{
    public class GetMovieRatingRequest : IRequest<MovieRatingGetDto>
    {
        public long RatingId { get; set; }
        public long MovieId { get; set; }
    }

    public class GetmovieRatingRequestHandler : IRequestHandler<GetMovieRatingRequest, MovieRatingGetDto>
    {
        private readonly DbContext _dbContext;
        private readonly IMapper _mapper;

        public GetmovieRatingRequestHandler(DbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<Response<MovieRatingGetDto>> HandleAsync(GetMovieRatingRequest request, CancellationToken token = default)
        {
           var rating = await  _dbContext.Set<MovieRating>()
               .AsNoTracking()
               .Include(x => x.Movie)
               .FirstOrDefaultAsync(x => x.Id == request.RatingId && x.MovieId == request.MovieId, token);

           return (rating == null) 
               ? Response<MovieRatingGetDto>.NotFound() 
               : _mapper.Map<MovieRatingGetDto>(rating).ToResponse();
        }
    }
}
