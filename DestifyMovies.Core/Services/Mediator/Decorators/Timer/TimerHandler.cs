using System.Diagnostics;
using Microsoft.Extensions.Logging;
using SimpleInjector;

namespace DestifyMovies.Core.Services.Mediator.Decorators.Timer
{
    //TODO: Possible Refactor logic to prevent duplication
    public class TimerHandler<TRequest, TResult> : IRequestHandler<TRequest, TResult>
    where TRequest : IRequest<TResult>
    {
        private readonly Func<IRequestHandler<TRequest, TResult>> _innerHandlerFactory;
        private readonly ILogger _logger;

        public TimerHandler(Func<IRequestHandler<TRequest, TResult>> innerHandlerFactory, ILogger logger)
        {
            _innerHandlerFactory = innerHandlerFactory;
            _logger = logger;
        }
        public async Task<Response<TResult>> HandleAsync(TRequest request, CancellationToken token = default)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await _innerHandlerFactory().HandleAsync(request, token);
            stopwatch.Stop();
            _logger.LogInformation($"Request: {typeof(TRequest).ToFriendlyName()} executed in {stopwatch.ElapsedMilliseconds} milliseconds");

            return result;
        }
    }

    public class TimerHandler<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        private readonly Func<IRequestHandler<TRequest>> _innerHandlerFactory;
        private readonly ILogger _logger;

        public TimerHandler(Func<IRequestHandler<TRequest>> innerHandlerFactory, ILogger logger)
        {
            _innerHandlerFactory = innerHandlerFactory;
            _logger = logger;
        }

        public async Task<Response<NoResult>> HandleAsync(TRequest request, CancellationToken token = default)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await _innerHandlerFactory().HandleAsync(request, token);
            stopwatch.Stop();
            _logger.LogInformation($"Request: {typeof(TRequest).ToFriendlyName()} executed in {stopwatch.ElapsedMilliseconds} milliseconds");

            return result;
        }
    }
}
