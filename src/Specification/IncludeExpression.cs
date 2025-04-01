using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Specification.Models;

namespace Specification;

public static class IncludeExpression
{
    public static IQueryable<T> Include<T>(this IQueryable<T> query, List<IncludeInfo> includes)
    {
        Expression queryExpression = query.Expression!;

        Span<IncludeInfo> includeInfos = CollectionsMarshal.AsSpan(includes);
        for (int i = 0; i < includeInfos.Length; i++)
        {
            IncludeInfo include = includeInfos[i];
            ParameterExpression parameter = Expression.Parameter(include.EntityType!, "x");

            string command =
                include.InCludeType == InCludeType.Include
                    ? nameof(EntityFrameworkQueryableExtensions.Include)
                    : nameof(EntityFrameworkQueryableExtensions.ThenInclude);

            List<Type> types = [include.EntityType!];

            if (include.InCludeType == InCludeType.Include)
            {
                types.Add(include.PropertyType!);
            }
            else
            {
                types.AddRange([include.PreviousPropertyType!, include.PropertyType!]);
            }

            queryExpression = Expression.Call(
                typeof(EntityFrameworkQueryableExtensions),
                command,
                [.. types],
                queryExpression,
                Expression.Quote(include.LamdaExpression!)
            );
        }

        return query.Provider.CreateQuery<T>(queryExpression);
    }
}
