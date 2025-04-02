using Specification.Interfaces;

namespace Specification.Evaluators;

public class SelectorSpecificationEvaluator
{
    public static IQueryable<TResponse> GetQuery<T, TResponse>(
        IQueryable<T> inputQuery,
        ISpecification<T, TResponse> specification
    )
        where T : class
        where TResponse : class
    {
        IQueryable<T> query = inputQuery;
        return query.Select(specification.Selector);
    }
}
