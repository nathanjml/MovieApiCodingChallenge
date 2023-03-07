using FluentValidation;

namespace DestifyMovies.Core.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderInitial<T, int> IsInRange<T>(this IRuleBuilderInitial<T, int> ruleBuilder, int lowerRangeInclusive, int upperRangeInclusive)
        {
            ruleBuilder.Custom((x, context) =>
            {
                var isInRange = x >= lowerRangeInclusive && x <= upperRangeInclusive;
                if(!isInRange)
                    context.AddFailure($"{context.PropertyName} is outside the bounds {lowerRangeInclusive} and {upperRangeInclusive}");
            });

            return ruleBuilder;
        }
    }
}
