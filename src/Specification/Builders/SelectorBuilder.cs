using System.Linq.Expressions;
using Specification.Interfaces;

namespace Specification.Builders;

public static class SelectorBuilder
{
    public static ISpecificationBuilder<T, TResponse> Select<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder,
        Expression<Func<T, TResponse>> selector
    )
        where T : class
        where TResponse : class
    {
        builder.Spec!.Selector = selector;

        return builder;
    }

    public static ISpecificationBuilder<T, TResponse> SelectMany<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder,
        Expression<Func<T, IEnumerable<TResponse>>> selector
    )
        where T : class
        where TResponse : class
    {
        builder.Spec!.SelectorMany = selector;

        return builder;
    }
}
