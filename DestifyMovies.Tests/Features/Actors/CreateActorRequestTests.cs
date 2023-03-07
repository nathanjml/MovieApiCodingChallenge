using System.Threading.Tasks;
using DestifyMovies.Core.Domain;
using DestifyMovies.Core.Features.Actors;
using DestifyMovies.Core.Features.Actors.Dtos;

namespace DestifyMovies.Tests.Features.Actors;

public class CreateActorRequestTests : CreateRequestTestFixture<CreateActorRequest, ActorGetDto, Actor>
{
    public override CreateActorRequest InitializeRequest()
    {
        return new CreateActorRequest
        {
            CountryOfOrigin = "Test",
            FirstName = "Test",
            LastName = "Test"
        };
    }

    public override Task SetupAsync()
    {
        return Task.CompletedTask;
    }
}