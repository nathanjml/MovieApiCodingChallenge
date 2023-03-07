using System;
using System.Threading.Tasks;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Features.MovieRatings;
using DestifyMovies.Core.Features.MovieRatings.Dtos;
using NUnit.Framework;

namespace DestifyMovies.Tests.Features.MovieRatings;

public class CreateMovieRatingTests : CreateRequestTestFixture<CreateMovieRatingRequest, MovieRatingGetDto, MovieRating>
{
    private long _id;

    public override async Task SetupAsync()
    {
        var movie = new Movie
        {
            Description = "Test",
            Director = "Test",
            ReleasedOn = new DateOnly(1, 1, 1),
            Title = "Test",
        };

        DbContext.Set<Movie>()
            .Add(movie);

        await DbContext.SaveChangesAsync();
        
        _id = movie.Id;
    }

    public override CreateMovieRatingRequest InitializeRequest()
    {
        return new CreateMovieRatingRequest
        {
            MovieId = _id,
            Comments = "Test",
            Rating = 3
        };
    }
}