using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Specification.Interfaces;
using Specification.Models;

namespace Specification.Evaluators;

public class SelectorSpecificationEvaluator
{
    public static IQueryable<TResponse> GetQuery<T, TResponse>(
        IQueryable<T> inputQuery,
        ISpecification<T, TResponse> specification
    )
        where T : class
        where TResponse : class
    {
        IQueryable<T> query = inputQuery;
        return Evaluate(query, specification);
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

        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        if (specification.Includes.Count > 0)
        {
            query = query.Include(specification.Includes);
        }

        if (specification.IsSplitQuery)
        {
            query = query.AsSplitQuery();
        }

        if (specification.Selector == null)
        {
            throw new Exception("Missing response mapping");
        }
        IQueryable<TResponse> queryResult = query.Select(specification.Selector);

        if (specification.Filter != null)
        {
            queryResult = queryResult.Where(specification.Filter);
        }

        if (specification.Search != null)
        {
            queryResult = queryResult.Where(specification.Search);
        }

        if (specification.Sorts.Count > 0)
        {
            Span<OrderByInfo<TResponse>> orderByInfos = CollectionsMarshal.AsSpan(
                specification.Sorts
            );
            IOrderedQueryable<TResponse> order = Enumerable
                .Empty<TResponse>()
                .AsQueryable()
                .OrderBy(x => 0);
            for (int i = 0; i < orderByInfos.Length; i++)
            {
                var orderbyInfo = orderByInfos[i];

                if (!orderbyInfo.IsThenBy)
                {
                    order =
                        orderbyInfo.OrderType == OrderType.Asc
                            ? queryResult.OrderBy(orderbyInfo.KeySelector)
                            : queryResult.OrderByDescending(orderbyInfo.KeySelector);
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
                queryResult = order;
            }
        }

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
