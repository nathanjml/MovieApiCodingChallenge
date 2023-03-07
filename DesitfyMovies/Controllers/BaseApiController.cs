using System.Net;
using DestifyMovies.Core.Services.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesitfyMovies.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "ApiKey")]
    public abstract class BaseApiController : ControllerBase
    {
        public IMediator Mediator { get; set; } = null!;

        protected async Task<IActionResult> HandleAsync(IRequest request,
            CancellationToken token,
            HttpStatusCode? successCode = null)
        {
            var response = await Mediator.HandleAsync(request, token);
            var responseStatus = successCode == null
                ? response.StatusCode
                : response.HasErrors
                    ? HttpStatusCode.BadRequest
                    : successCode.Value;

            return HandleResult(response, responseStatus);
        }

        protected async Task<IActionResult> HandleAsync<T>(IRequest<T> request,
            CancellationToken token,
            HttpStatusCode? successCode = null)
        {
            var response = await Mediator.HandleAsync(request, token);
            var responseStatus = successCode == null
                ? response.StatusCode
                : response.HasErrors
                    ? HttpStatusCode.BadRequest
                    : successCode.Value;

            return HandleResult(response, responseStatus);
        }

        private IActionResult HandleResult<T>(Response<T> response, HttpStatusCode successCode)
        {
            response.StatusCode = successCode;

            return StatusCode((int)successCode, response);
        }
    }
}
