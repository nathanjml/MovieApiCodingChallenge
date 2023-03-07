using DestifyMovies.Core.Services.Mediator;

namespace DestifyMovies.Core.Extensions
{
    public static class ResponseExtensions
    {
        public static Response<TResult> ToResponse<TResult>(this TResult result) 
            => new() {Result = result};

        public static Task<Response<TResult>> ToResponseAsync<TResult>(this TResult result) 
            => Task.FromResult(ToResponse(result));
    }
}
