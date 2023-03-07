namespace DestifyMovies.Core.Services.Mediator.Decorators.Validation
{
    public interface IRequestValidator<in TRequest>
    {
        Task<List<Error>?> ValidateAsync(TRequest request, CancellationToken token = default);
    }
}
