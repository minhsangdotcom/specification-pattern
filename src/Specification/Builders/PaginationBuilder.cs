using Specification.Interfaces;

namespace Specification.Builders;

public static class PaginationBuilder
{
    public static ISpecificationBuilder<T, TResponse> Skip<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder,
        int skip
    )
        where T : class
        where TResponse : class
    {
        builder.Spec!.Skip = skip;
        return builder;
    }

    public static ISpecificationBuilder<T, TResponse> Take<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder,
        int take
    )
        where T : class
        where TResponse : class
    {
        builder.Spec!.Skip = take;
        return builder;
    }
}
