using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Specification.Interfaces;
using Specification.Models;

namespace Specification.Evaluators;

public class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(
        IQueryable<T> inputQuery,
        ISpecification<T> specification
    )
        where T : class
    {
        IQueryable<T> query = inputQuery;
        return Evaluate(query, specification);
    }

    public static string GetStringQuery<T>(ISpecification<T> specification)
        where T : class
    {
        IQueryable<T> query = Enumerable.Empty<T>().AsQueryable();
        query = Evaluate(query, specification);
        return query.Expression.ToString();
    }

    private static IQueryable<T> Evaluate<T>(IQueryable<T> query, ISpecification<T> specification)
        where T : class
    {
        if (specification.IsNoTracking)
        {
            query = query.AsNoTracking();
        }

        if (specification.Criteria.Count > 0)
        {
            query = specification.Criteria.Aggregate(
                query,
                (current, criteria) => current.Where(criteria.Criteria)
            );
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

        if (specification.Skip > 0)
        {
            query = query.Skip(specification.Skip);
        }

        if (specification.Take >= 0)
        {
            query = query.Take(specification.Take);
        }

        return query;
    }
}
