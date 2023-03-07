using DestifyMovies.Core.Features.Users;
using DestifyMovies.Core.Features.Users.Dtos;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesitfyMovies.Controllers
{
    
    public class UsersController : BaseApiController
    {
        [HttpGet("me")]
        [Produces(typeof(Response<UserDto>))]
        public Task<IActionResult> CurrentUser(CancellationToken token)
            => HandleAsync(new GetCurrentUserRequest(), token);

        [HttpPost("users")]
        [Produces(typeof(Response<string>))]
        [AllowAnonymous]
        public Task<IActionResult> CreateUser([FromBody] CreateUserRequest request, CancellationToken token)
            => HandleAsync(request, token);


        [HttpPut("me/regenerate-token")]
        [Produces(typeof(Response<string>))]
        public Task<IActionResult> RegenerateUserToken(CancellationToken token)
            => HandleAsync(new RegenerateUserTokenRequest(), token);
    }
}
