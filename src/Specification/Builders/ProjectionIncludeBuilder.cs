using System.Linq.Expressions;
using Specification.Interfaces;
using Specification.Models;

namespace Specification.Builders;

public static class ProjectionIncludeBuilder
{
    public static IIncludableSpecificationBuilder<T, TResponse, TProperty> Include<
        T,
        TResponse,
        TProperty
    >(
        this ISpecificationBuilder<T, TResponse> builder,
        Expression<Func<T, TProperty>> includeExpression
    )
        where T : class
        where TResponse : class
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

        return new IncludableSpecificationBuilder<T, TResponse, TProperty>(builder.Spec);
    }

    public static IIncludableSpecificationBuilder<T, TResponse, TProperty> ThenInclude<
        T,
        TResponse,
        TPreviousProperty,
        TProperty
    >(
        this IIncludableSpecificationBuilder<T, TResponse, TPreviousProperty> builder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression
    )
        where T : class
        where TResponse : class
    {
        return ThenIncludeBase(thenIncludeExpression, builder);
    }

    //Then include with collection is previous type
    public static IIncludableSpecificationBuilder<T, TResponse, TProperty> ThenInclude<
        T,
        TResponse,
        TPreviousProperty,
        TProperty
    >(
        this IIncludableSpecificationBuilder<T, TResponse, IEnumerable<TPreviousProperty>> builder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression
    )
        where T : class
        where TResponse : class
    {
        return ThenIncludeBase(thenIncludeExpression, collectionBuilder: builder);
    }

    private static IIncludableSpecificationBuilder<T, TResponse, TProperty> ThenIncludeBase<
        T,
        TResponse,
        TPreviousProperty,
        TProperty
    >(
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression,
        IIncludableSpecificationBuilder<T, TResponse, TPreviousProperty> builder = null!,
        IIncludableSpecificationBuilder<
            T,
            TResponse,
            IEnumerable<TPreviousProperty>
        > collectionBuilder = null!
    )
        where T : class
        where TResponse : class
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
        Specification<T, TResponse>? Spec = builder != null ? builder.Spec : collectionBuilder.Spec;
        Spec!.Includes.Add(includeInfo);

        IIncludableSpecificationBuilder<T, TResponse, TProperty> includeBuilder =
            new IncludableSpecificationBuilder<T, TResponse, TProperty>(Spec);
        return includeBuilder;
    }
}
