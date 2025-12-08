using System.Linq.Expressions;
using Specification.Interfaces;
using Specification.Models;

namespace Specification.Builders;

public static class IncludeBuilder
{
    public static IIncludableSpecificationBuilder<T, TProperty> Include<T, TProperty>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, TProperty>> includeExpression
    )
        where T : class
    {
        IncludeInfo includeInfo =
            new()
            {
                EntityType = typeof(T),
                InCludeType = InCludeType.Include,
                LamdaExpression = includeExpression,
                PropertyType = typeof(TProperty),
            };

        builder.Spec!.Includes.Add(includeInfo);

        return new IncludableSpecificationBuilder<T, TProperty>(builder.Spec);
    }

    public static IIncludableSpecificationBuilder<T, TProperty> ThenInclude<
        T,
        TPreviousProperty,
        TProperty
    >(
        this IIncludableSpecificationBuilder<T, TPreviousProperty> builder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression
    )
        where T : class
    {
        return ThenIncludeBase(thenIncludeExpression, builder);
    }

    //Then include with collection is previous type
    public static IIncludableSpecificationBuilder<T, TProperty> ThenInclude<
        T,
        TPreviousProperty,
        TProperty
    >(
        this IIncludableSpecificationBuilder<T, IEnumerable<TPreviousProperty>> builder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression
    )
        where T : class
    {
        return ThenIncludeBase(thenIncludeExpression, CollectionBuilder: builder);
    }

    private static IIncludableSpecificationBuilder<T, TProperty> ThenIncludeBase<
        T,
        TPreviousProperty,
        TProperty
    >(
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression,
        IIncludableSpecificationBuilder<T, TPreviousProperty> builder = null!,
        IIncludableSpecificationBuilder<T, IEnumerable<TPreviousProperty>> CollectionBuilder = null!
    )
        where T : class
    {
        IncludeInfo includeInfo =
            new()
            {
                EntityType = typeof(T),
                InCludeType = InCludeType.ThenInclude,
                LamdaExpression = thenIncludeExpression,
                PreviousPropertyType = typeof(TPreviousProperty),
                PropertyType = typeof(TProperty),
            };
        Specification<T>? Spec = builder != null ? builder.Spec : CollectionBuilder.Spec;
        Spec!.Includes.Add(includeInfo);

        IIncludableSpecificationBuilder<T, TProperty> includeBuilder =
            new IncludableSpecificationBuilder<T, TProperty>(Spec);
        return includeBuilder;
    }
}
