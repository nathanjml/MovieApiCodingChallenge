using DestifyMovies.Core.Features.Actors;
using DestifyMovies.Core.Features.Actors.Dtos;
using DestifyMovies.Core.Features.Movies.Dtos;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesitfyMovies.Controllers
{
    [Route("actors")]
    public class ActorsController : BaseApiController
    {
        [HttpGet]
        [Produces(typeof(Response<List<ActorGetDto>>))]
        [AllowAnonymous]
        public Task<IActionResult> GetAllActors([FromQuery] GetActorsRequest request, CancellationToken token)
            => HandleAsync(request, token);

        [HttpGet("{id:long}")]
        [Produces(typeof(Response<ActorGetDto>))]
        [AllowAnonymous]
        public Task<IActionResult> GetActor(long id, CancellationToken token)
            => HandleAsync(new GetActorRequest { Id = id }, token);

        [HttpGet("{id:long}/movies")]
        [Produces(typeof(Response<ListMovieDto>))]
        [AllowAnonymous]
        public Task<IActionResult> GetActorMovies(long id, CancellationToken token)
            => HandleAsync(new GetActorMoviesRequest { Id = id }, token);

        [HttpDelete("{id:long}")]
        [Produces(typeof(Response))]
        public Task<IActionResult> DeleteActor(long id, CancellationToken token)
            => HandleAsync(new DeleteActorRequest { Id = id }, token);


        [HttpPost]
        [Produces(typeof(Response<ActorGetDto>))]
        public Task<IActionResult> CreateActor([FromBody] CreateActorRequest request,
            CancellationToken token) =>
            HandleAsync(request, token);

        [HttpPut("{id:long}")]
        [Produces(typeof(Response<ActorGetDto>))]
        public Task<IActionResult> UpdateActor(long id, [FromBody] ActorEditDto dto, CancellationToken token)
        {
            var request = new UpdateActorRequest(id, dto);
            return HandleAsync(request, token);
        }
    }
}
