
using System.Net;
using SimpleInjector;

namespace DestifyMovies.Core.Services.Mediator.Decorators.Validation;

//TODO: Possible Refactor logic to prevent duplication
public class ValidationHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    private readonly Func<IRequestHandler<TRequest, TResult>> _innerHandlerFactory;
    private readonly ValidatorFactory _validatorFactory;

    public ValidationHandler(Func<IRequestHandler<TRequest, TResult>> innerHandlerFactory, ValidatorFactory validatorFactory)
    {
        _innerHandlerFactory = innerHandlerFactory;
        _validatorFactory = validatorFactory;
    }

    private static string RequestName => typeof(TRequest).ToFriendlyName();

    public async Task<Response<TResult>> HandleAsync(TRequest request, CancellationToken token)
    {

        var validator = _validatorFactory.TryCreate<TRequest, TResult>();
        if (validator == null)
            return await _innerHandlerFactory().HandleAsync(request, token);

        var validationResult = await validator.ValidateAsync(request, token);
        if (validationResult == null || validationResult.Count == 0)
        {
            return await _innerHandlerFactory().HandleAsync(request, token);
        }

        return new Response<TResult>
        {
            Errors = validationResult,
            StatusCode = HttpStatusCode.BadRequest
        };
    }
}

public class ValidationHandler<TRequest> : IRequestHandler<TRequest>
    where TRequest : IRequest
{
    private readonly Func<IRequestHandler<TRequest>> _innerHandlerFactory;
    private readonly ValidatorFactory _validatorFactory;

    public ValidationHandler(Func<IRequestHandler<TRequest>> innerHandlerFactory, ValidatorFactory validatorFactory)
    {
        _innerHandlerFactory = innerHandlerFactory;
        _validatorFactory = validatorFactory;
    }

    public async Task<Response<NoResult>> HandleAsync(TRequest request, CancellationToken token = default)
    {
        var validator = _validatorFactory.TryCreate<TRequest>();
        if (validator == null)
            return await _innerHandlerFactory().HandleAsync(request, token);

        var validationResult = await validator.ValidateAsync(request, token);
        if (validationResult == null || validationResult.Count == 0)
        {
            return await _innerHandlerFactory().HandleAsync(request, token);
        }

        return new Response
        {
            Errors = validationResult,
            StatusCode = HttpStatusCode.BadRequest
        };
    }
}




