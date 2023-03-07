using FluentValidation;

namespace DestifyMovies.Core.Services.Mediator.Decorators.Validation
{
    public class FluentValidatorAdapter<T> : IRequestValidator<T>
    {
        private readonly IValidator<T> _validator;
        public FluentValidatorAdapter(IValidator<T> validator)
        {
            _validator = validator;
        }
        public async Task<List<Error>?> ValidateAsync(T request, CancellationToken token = default)
        {
            var validationResult = await _validator.ValidateAsync(request, token);
            if (validationResult.IsValid)
                return null;

            return validationResult.Errors
                .Select(error => new Error
                {
                    ErrorMessage = error.ErrorMessage,
                    PropertyName = error.PropertyName
                })
                .ToList();
        }
    }
}
