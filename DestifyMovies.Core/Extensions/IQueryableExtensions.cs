using System.Linq.Expressions;

namespace DestifyMovies.Core.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable
            , Func<bool> condition
            , Expression<Func<T, bool>> predicate) =>
            condition() ? queryable.Where(predicate) : queryable;
    }
}
