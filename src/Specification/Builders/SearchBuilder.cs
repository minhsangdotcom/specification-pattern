using System.Linq.Expressions;
using Specification.Interfaces;

namespace Specification.Builders;

public static class SearchBuilder
{
    public static ISpecificationBuilder<T, TResponse> Search<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder,
        Expression<Func<TResponse, bool>> search
    )
        where T : class
        where TResponse : class
    {
        builder.Spec!.Search = search;
        return builder;
    }
}
