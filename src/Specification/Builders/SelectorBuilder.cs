using Specification.Interfaces;

namespace Specification.Builders;

public static class SelectorBuilder
{
    public static ISpecificationBuilder<T, TResponse> Select<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder,
        TResponse response
    )
        where T : class
        where TResponse : class
    {
        builder.Spec!.Selector = x => response;

        return builder;
    }
}
