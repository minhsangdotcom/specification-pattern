using System.Linq.Expressions;
using Specification.Interfaces;
using Specification.Models;

namespace Specification.Builders;

public static class OrderbyBuilder
{
    public static ISpecificationBuilder<T, TResponse> OrderBy<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder,
        Expression<Func<TResponse, object>> orderby
    )
        where T : class
        where TResponse : class
    {
        var orderbyInfo = new OrderByInfo<TResponse>()
        {
            KeySelector = orderby,
            OrderType = OrderType.Asc,
        };
        builder.Spec!.Sorts.Add(orderbyInfo);
        return builder;
    }

    public static ISpecificationBuilder<T, TResponse> OrderByDescending<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder,
        Expression<Func<TResponse, object>> orderby
    )
        where T : class
        where TResponse : class
    {
        var orderbyInfo = new OrderByInfo<TResponse>()
        {
            KeySelector = orderby,
            OrderType = OrderType.Desc,
        };
        builder.Spec!.Sorts.Add(orderbyInfo);
        return builder;
    }

    public static ISpecificationBuilder<T, TResponse> ThenBy<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder,
        Expression<Func<TResponse, object>> orderby
    )
        where T : class
        where TResponse : class
    {
        var orderbyInfo = new OrderByInfo<TResponse>()
        {
            KeySelector = orderby,
            OrderType = OrderType.Asc,
            IsThenBy = true,
        };
        builder.Spec!.Sorts.Add(orderbyInfo);
        return builder;
    }

    public static ISpecificationBuilder<T, TResponse> ThenByDescending<T, TResponse>(
        this ISpecificationBuilder<T, TResponse> builder,
        Expression<Func<TResponse, object>> orderby
    )
        where T : class
        where TResponse : class
    {
        var orderbyInfo = new OrderByInfo<TResponse>()
        {
            KeySelector = orderby,
            OrderType = OrderType.Desc,
            IsThenBy = true,
        };
        builder.Spec!.Sorts.Add(orderbyInfo);
        return builder;
    }
}
