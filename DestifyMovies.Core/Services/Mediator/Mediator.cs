namespace DestifyMovies.Core.Services.Mediator
{
    public class Mediator : IMediator
    {

        private readonly Func<Type, object> _resolver;

        public Mediator(Func<Type, object> resolver)
        {
            _resolver = resolver;
        }

        public Task<Response> HandleAsync(IRequest request, CancellationToken token)
        {
            var handlerType = typeof(IRequestHandler<>).MakeGenericType(request.GetType());
            return (Task<Response>) HandleBase(handlerType, request);
        }

        public Task<Response<T>> HandleAsync<T>(IRequest<T> request, CancellationToken token)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(T));
            return (Task<Response<T>>)HandleBase(handlerType, request);
        }

        private object HandleBase(Type handlerType, dynamic request)
        {
            dynamic handler = _resolver(handlerType);
            return handler.HandleAsync(request);
        }
    }
}
