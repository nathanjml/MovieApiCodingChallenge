using DestifyMovies.Core.Features.Actors.Dtos;
using DestifyMovies.Core.Features.MovieRatings;
using DestifyMovies.Core.Features.MovieRatings.Dtos;
using DestifyMovies.Core.Features.Movies;
using DestifyMovies.Core.Features.Movies.Dtos;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesitfyMovies.Controllers;

[Route("movies")]
public class MoviesController : BaseApiController
{
    [HttpGet]
    [Produces(typeof(Response<List<ListMovieDto>>))]
    [AllowAnonymous]
    public Task<IActionResult> GetAllMovies([FromQuery] GetMoviesRequest request, CancellationToken token)
        => HandleAsync(request, token);

    [HttpGet("{id:long}")]
    [Produces(typeof(Response<MovieGetDto>))]
    [AllowAnonymous]
    public Task<IActionResult> GetMovie(long id, CancellationToken token)
        => HandleAsync(new GetMovieRequest{Id = id}, token);

    [HttpDelete("{id:long}")]
    [Produces(typeof(Response))]
    public Task<IActionResult> DeleteMovie(long id, CancellationToken token)
        => HandleAsync(new DeleteMovieRequest {Id = id}, token);

    [HttpPut("{id:long}")]
    [Produces(typeof(Response<MovieGetDto>))]
    public Task<IActionResult> UpdateMovie(long id, [FromBody]MovieEditDto movieEditDto, CancellationToken token)
    {
        var request = new UpdateMovieRequest(id, movieEditDto);
        return HandleAsync(request, token);
    }

    [HttpPost]
    [Produces(typeof(Response<MovieGetDto>))]
    public Task<IActionResult> CreateMovie([FromBody] CreateMovieRequest request, CancellationToken token)
        => HandleAsync(request, token);

    //movie ratings

    [HttpGet("{id:long}/ratings")]
    [Produces(typeof(Response<List<ListMovieRatingDto>>))]
    [AllowAnonymous]
    public Task<IActionResult> GetMovieRatings(long id, CancellationToken token)
        => HandleAsync(new GetMovieRatingsRequest {MovieId = id}, token);

    [HttpGet("{movieId:long}/ratings/{ratingId:long}")]
    [Produces(typeof(Response<List<ListMovieRatingDto>>))]
    [AllowAnonymous]
    public Task<IActionResult> GetMovieRating(long movieId, long ratingId, CancellationToken token)
        => HandleAsync(new GetMovieRatingRequest() { RatingId = ratingId, MovieId = movieId}, token);

    [HttpPost("{id:long}/ratings")]
    [Produces(typeof(Response<MovieRatingGetDto>))]
    public Task<IActionResult> CreateMovieRating(long id, [FromBody] MovieRatingDto dto,
        CancellationToken token)
    {
        var request = new CreateMovieRatingRequest
        {
            MovieId = id,
            Comments = dto.Comments,
            Rating = dto.Rating
        };

        return HandleAsync(request, token);
    }

    [HttpDelete("{movieId:long}/ratings/{ratingId:long}")]
    [Produces(typeof(Response))]
    public Task<IActionResult> DeleteMovieRating(long movieId, long ratingId, CancellationToken token)
        => HandleAsync(new DeleteMovieRatingRequest { MovieId = movieId, RatingId = ratingId}, token);

    [HttpPut("{movieId:long}/ratings/{ratingId:long}")]
    [Produces(typeof(Response<MovieRatingGetDto>))]
    public Task<IActionResult> EditMovieRating(long movieId, long ratingId, MovieRatingDto dto, CancellationToken token)
    {
        var request = new UpdateMovieRatingsRequest(movieId, ratingId, dto);
        return HandleAsync(request, token);
    }

    //movie actors

    [HttpDelete("{movieId:long}/actors/{actorId:long}")]
    [Produces(typeof(Response))]
    public Task<IActionResult> RemoveMovieActor(long movieId, long actorId, CancellationToken token)
        => HandleAsync(new RemoveMovieActorRequest { MovieId = movieId, ActorId = actorId }, token);

    [HttpPut("{movieId:long}/actors/{actorId:long}")]
    [Produces(typeof(Response<MovieGetDto>))]
    public Task<IActionResult> AddMovieActor(long movieId, long actorId, CancellationToken token)
        => HandleAsync(new AddMovieActorRequest { MovieId = movieId, ActorId = actorId }, token);

    [HttpGet("{id:long}/actors")]
    [Produces(typeof(Response<List<ActorGetDto>>))]
    [AllowAnonymous]
    public Task<IActionResult> GetMovieActors(long id, CancellationToken token)
        => HandleAsync(new GetMovieActorsRequest {Id = id}, token);
}