using System.Net;
using DestifyMovies.Core.Identity;

namespace DestifyMovies.Core.Services.Mediator;

public sealed class NoResult
{
}

public class Response : Response<NoResult>
{
}
public class Response<T>
{
    public bool HasErrors => Errors.Any();
    public List<Error> Errors { get; set; } = new List<Error>();
    public T? Result { get; set; }
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

    public static implicit operator Response(Response<T> response)
    {
        Response newResponse = new Response
        {
            Errors = response.Errors,
            StatusCode = response.StatusCode
        };
        return newResponse;
    }

    public static Response<T> Success() => new()
    {
        StatusCode = HttpStatusCode.OK
    };

    public static Response<T> Created() => new()
    {
        StatusCode = HttpStatusCode.Created
    };

    public static Response<T> Created(T result) => new()
    {
        Result = result,
        StatusCode = HttpStatusCode.Created
    };

    public static Response<T> Forbidden() => new()
    {
        StatusCode = HttpStatusCode.Created
    };

    public static Response<T> NotFound() => new()
    {
        StatusCode = HttpStatusCode.NotFound
    };

    public static Task<Response<T>> NotFoundAsync() => Task.FromResult(NotFound());

    public static Response<T> BadRequest() => new()
    {
        StatusCode = HttpStatusCode.BadRequest
    };

    public static Task<Response<T>> BadRequestAsync() => Task.FromResult(BadRequest());
}
