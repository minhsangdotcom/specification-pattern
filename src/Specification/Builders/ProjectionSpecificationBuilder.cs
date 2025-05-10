using System.Linq.Expressions;
using Specification.Interfaces;
using Specification.Models;

namespace Specification.Builders;

public static class ProjectionSpecificationBuilder
{
    public static ISpecificationBuilder<T, TResponse> Where<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder,
        List<CriteriaInfo<T>> criteria
    )
        where T : class
        where TResponse : class
    {
        builder.Spec!.Criteria = criteria;

        return builder;
    }

    public static ISpecificationBuilder<T, TResponse> Combine<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder,
        List<CriteriaInfoUpdate<T>> criteria
    )
        where T : class
        where TResponse : class
    {
        builder.Spec!.CombineExpression(criteria);

        return builder;
    }

    public static ISpecificationBuilder<T, TResponse> AsSplitQuery<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder
    )
        where T : class
        where TResponse : class
    {
        builder.Spec!.IsSplitQuery = true;

        return builder;
    }

    public static ISpecificationBuilder<T, TResponse> AsNoTracking<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder
    )
        where T : class
        where TResponse : class
    {
        builder.Spec!.IsNoTracking = true;

        return builder;
    }

    public static void EnableCache<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder,
        string cacheKey
    )
        where T : class
        where TResponse : class
    {
        builder.Spec!.CacheEnabled = true;
        builder.Spec.CacheKey = cacheKey;
    }
}
