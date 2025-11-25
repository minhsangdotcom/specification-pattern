using System.Linq.Expressions;
using Specification.Interfaces;
using Specification.Models;

namespace Specification.Builders;

public static class SpecificationBaseBuilder
{
    public static ISpecificationBuilder<T> Where<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, bool>> expression
    )
        where T : class
    {
        builder.Spec!.Wheres.Add(new WhereInfo<T>(expression));
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
