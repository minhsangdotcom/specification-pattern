using Specification.Interfaces;

namespace Specification.Builders;

public static class PaginationBuilder
{
    #region T ver
    public static ISpecificationBuilder<T> Skip<T>(this ISpecificationBuilder<T> builder, int skip)
        where T : class
    {
        builder.Spec!.Skip = skip;
        return builder;
    }

    public static ISpecificationBuilder<T> Take<T>(this ISpecificationBuilder<T> builder, int take)
        where T : class
    {
        builder.Spec!.Skip = take;
        return builder;
    }
    #endregion

    #region T, TResponse ver
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
    #endregion
}
