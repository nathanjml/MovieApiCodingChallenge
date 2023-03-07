using AutoMapper;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Extensions;
using DestifyMovies.Core.Features.Movies.Dtos;
using DestifyMovies.Core.Services.Mediator;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DestifyMovies.Core.Features.Movies;

public class UpdateMovieRequest : IRequest<MovieGetDto>
{
    public UpdateMovieRequest(long movieId, MovieEditDto movieEditDto)
    {
        MovieId = movieId;
        MovieDto = movieEditDto;
    }

    public long MovieId { get; set; }
    public MovieEditDto MovieDto { get; set; }
}

public class UpdateMovieRequestValidator : AbstractValidator<UpdateMovieRequest>
{
    public UpdateMovieRequestValidator()
    {
        RuleFor(x => x.MovieDto.Description).NotEmpty();
        RuleFor(x => x.MovieDto.Title).NotEmpty();
        RuleFor(x => x.MovieDto.ReleasedOn)
            .NotEmpty()
            .DependentRules(() =>
            {
                RuleFor(y => y.MovieDto.ReleasedOn).Must(str => DateOnly.TryParse(str, out var result));
            });
    }
}

public class UpdateMovieRequestHandler : IRequestHandler<UpdateMovieRequest, MovieGetDto>
{
    private readonly DbContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateMovieRequestHandler(DbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Response<MovieGetDto>> HandleAsync(UpdateMovieRequest request, CancellationToken token = default)
    {
        var movie = await _dbContext.Set<Movie>()
            .FirstOrDefaultAsync(x => x.Id == request.MovieId, token);

        if(movie == null)
            return Response<MovieGetDto>.NotFound();

        _mapper.Map(request.MovieDto, movie);

        await _dbContext.SaveChangesAsync(token);

        return _mapper.Map<MovieGetDto>(movie).ToResponse();
    }
}