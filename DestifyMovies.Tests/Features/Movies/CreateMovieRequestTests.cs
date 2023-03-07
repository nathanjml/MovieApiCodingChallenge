using System.Threading.Tasks;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Features.Movies;
using DestifyMovies.Core.Features.Movies.Dtos;

namespace DestifyMovies.Tests.Features.Movies;

public class CreateMovieRequestTests : CreateRequestTestFixture<CreateMovieRequest, MovieGetDto, Movie>
{
    public override CreateMovieRequest InitializeRequest()
    {
        return new CreateMovieRequest
        {
            Description = "Test",
            Director = "Test",
            Title = "Test",
            ReleasedOn = "12/12/2000",
        };
    }

    public override Task SetupAsync()
    {
        return Task.CompletedTask;
    }
}