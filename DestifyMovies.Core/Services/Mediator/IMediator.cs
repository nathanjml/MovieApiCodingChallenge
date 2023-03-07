namespace DestifyMovies.Core.Services.Mediator;
public interface IMediator
{
    Task<Response<T>> HandleAsync<T>(IRequest<T> request, CancellationToken token = default);
}
