using System.Linq.Expressions;
using Specification.Interfaces;
using Specification.Models;

namespace Specification.Builders;

public static class SpecificationBaseBuilder
{
    public static ISpecificationBuilder<T> Where<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, bool>> expression,
        string? key = null
    )
        where T : class
    {
        builder.Spec!.Criteria.Add(new CriteriaInfo<T>(key, expression));
        return builder;
    }

    public static ISpecificationBuilder<T> Combine<T>(
        this ISpecificationBuilder<T> builder,
        List<CriteriaInfoUpdate<T>> criteria
    )
        where T : class
    {
        builder.Spec!.CombineExpression(criteria);

        return builder;
    }

    public static ISpecificationBuilder<T> AsSplitQuery<T>(this ISpecificationBuilder<T> builder)
        where T : class
    {
        builder.Spec!.IsSplitQuery = true;

        return builder;
    }

    public static ISpecificationBuilder<T> AsNoTracking<T>(this ISpecificationBuilder<T> builder)
        where T : class
    {
        builder.Spec!.IsNoTracking = true;

        return builder;
    }

    public static void EnableCache<T>(this ISpecificationBuilder<T> builder, string cacheKey)
        where T : class
    {
        builder.Spec!.CacheEnabled = true;
        builder.Spec.CacheKey = cacheKey;
    }
}