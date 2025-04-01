using System.Linq.Expressions;
using Specification.Interfaces;
using Specification.Models;

namespace Specification.Builders;

public static class SelectorIncludeBuilder
{
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

    public static IIncludableSpecificationBuilder<T, TResponse, TProperty> ThenInclude<
        T,
        TResponse,
        TPreviousProperty,
        TProperty
    >(
        this IIncludableSpecificationBuilder<T, TResponse, ICollection<TPreviousProperty>> builder,
        Expression<Func<TPreviousProperty, TProperty>> thenIncludeExpression
    )
        where T : class
        where TResponse : class
    {
        return ThenIncludeBase(thenIncludeExpression, Collectionbuilder: builder);
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
            ICollection<TPreviousProperty>
        > Collectionbuilder = null!
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

        Specification<T, TResponse>? Spec = builder != null ? builder.Spec : Collectionbuilder.Spec;

        Spec!.Includes.Add(includeInfo);

        return new IncludableSpecificationBuilder<T, TResponse, TProperty>(Spec);
    }
}
