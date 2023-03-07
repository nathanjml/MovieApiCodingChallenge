namespace DestifyMovies.Core.Services.Mediator;
public interface IRequest<TResult>
{
}

public interface IRequest : IRequest<NoResult>
{
}

public interface IRequestHandler<in TRequest, TResult> where TRequest : IRequest<TResult>
{
    Task<Response<TResult>> HandleAsync(TRequest request, CancellationToken token = default);
}

public interface IRequestHandler<in TRequest>
    where TRequest : IRequest<NoResult>
{
    Task<Response<NoResult>> HandleAsync(TRequest request, CancellationToken token = default);
}
