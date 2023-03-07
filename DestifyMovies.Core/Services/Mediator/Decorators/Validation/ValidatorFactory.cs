using SimpleInjector;

namespace DestifyMovies.Core.Services.Mediator.Decorators.Validation;
public class ValidatorFactory
{
    private readonly Container _container;

    public ValidatorFactory(Container container)
    {
        _container = container;
    }

    public IRequestValidator<TRequest>? TryCreate<TRequest, TResult>() 
        where TRequest : IRequest<TResult>
    {
        try
        {
            return _container.GetInstance<IRequestValidator<TRequest>>();
        }
        catch (Exception)
        {
            return null;
        }
    }

    public IRequestValidator<TRequest>? TryCreate<TRequest>()
    where TRequest : IRequest
    {
        return TryCreate<TRequest, NoResult>();
    }
}
