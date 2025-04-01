using System.Linq.Expressions;
using Specification.Interfaces;

namespace Specification.Builders;

public static class FIlterBuilder
{
    public static ISpecificationBuilder<T, TResponse> Filter<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder,
        Expression<Func<TResponse, bool>> filter
    )
        where T : class
        where TResponse : class
    {
        builder.Spec!.Filter = filter;
        return builder;
    }
}
