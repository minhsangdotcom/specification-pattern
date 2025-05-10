using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Specification.Interfaces;
using Specification.Models;

namespace Specification.Evaluators;

public class ProjectionSpecificationEvaluator
{
    public static IQueryable<TResponse> GetQuery<T, TResponse>(
        IQueryable<T> inputQuery,
        ISpecification<T, TResponse> specification
    )
        where T : class
        where TResponse : class
    {
        return Evaluate(inputQuery, specification);
    }

    public static string SpecStringQuery<T, TResponse>(ISpecification<T, TResponse> specification)
        where T : class
        where TResponse : class
    {
        IQueryable<TResponse> query = Evaluate(Enumerable.Empty<T>().AsQueryable(), specification);
        return query.Expression.ToString();
    }

    private static IQueryable<TResponse> Evaluate<T, TResponse>(
        IQueryable<T> query,
        ISpecification<T, TResponse> specification
    )
        where T : class
        where TResponse : class
    {
        if (specification.IsNoTracking)
        {
            query = query.AsNoTracking();
        }

        if (specification.Criteria.Count > 0)
        {
            query = specification.Criteria.Aggregate(query, (current, criteria) =>
                current.Where(criteria.Criteria));
        }

        if (specification.Includes.Count > 0)
        {
            query = query.Include(specification.Includes);
        }

        if (specification.IsSplitQuery)
        {
            query = query.AsSplitQuery();
        }

        if (specification.Sorts.Count > 0)
        {
            Span<OrderByInfo<T>> orderByInfos = CollectionsMarshal.AsSpan(specification.Sorts);
            IOrderedQueryable<T>? order = null!;
            for (int i = 0; i < orderByInfos.Length; i++)
            {
                var orderbyInfo = orderByInfos[i];

                if (order == null)
                {
                    order =
                        orderbyInfo.OrderType == OrderType.Asc
                            ? query.OrderBy(orderbyInfo.KeySelector)
                            : query.OrderByDescending(orderbyInfo.KeySelector);
                }
                else
                {
                    order =
                        orderbyInfo.OrderType == OrderType.Asc
                            ? order.ThenBy(orderbyInfo.KeySelector)
                            : order.ThenByDescending(orderbyInfo.KeySelector);
                }
            }

            if (order.Any())
            {
                query = order;
            }
        }

        if (specification.Selector == null)
        {
            throw new Exception("Missing response mapping");
        }

        IQueryable<TResponse> queryResult = SelectorSpecificationEvaluator.GetQuery(
            query,
            specification
        );

        if (specification.Skip > 0)
        {
            queryResult = queryResult.Skip(specification.Skip);
        }

        if (specification.Take >= 0)
        {
            queryResult = queryResult.Take(specification.Take);
        }

        return queryResult;
    }
}