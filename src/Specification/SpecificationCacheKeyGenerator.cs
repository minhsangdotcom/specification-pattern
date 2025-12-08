using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Specification.Interfaces;
using Specification.Models;

namespace Specification;

public class SpecificationCacheKeyGenerator
{
    private static readonly JsonSerializerOptions JsonOptions =
        new()
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

    public static string Create<T>(ISpecification<T> spec, params object[]? queryParameters)
        where T : class
    {
        var meta = BuildMetadata(spec, queryParameters);
        string json = JsonSerializer.Serialize(meta, JsonOptions);
        return json;
    }

    public static string Create<T, TResponse>(
        ISpecification<T, TResponse> spec,
        params object[]? queryParameters
    )
        where T : class
        where TResponse : class
    {
        var meta = BuildMetadata(spec, queryParameters);

        string json = JsonSerializer.Serialize(meta, JsonOptions);
        return json;
    }

    private static SpecCacheMetadata BuildMetadata<T>(
        ISpecification<T> spec,
        params object[]? queryParameters
    )
        where T : class
    {
        List<WhereMeta> wheres =
        [
            .. spec.Wheres.Select(w => new WhereMeta
            {
                Expression = SafeExpressionToString(w.Filter),
            }),
        ];

        List<IncludeMeta> includes =
        [
            .. spec.Includes.Select(i => new IncludeMeta
            {
                Expression = SafeExpressionToString(i.LamdaExpression),
                PropertyType = i.PropertyType?.FullName,
                EntityType = i.EntityType?.FullName,
                PreviousPropertyType = i.PreviousPropertyType?.FullName,
                InCludeType = i.InCludeType,
            }),
        ];

        List<SortMeta> sorts =
        [
            .. spec.Sorts.Select(s => new SortMeta
            {
                Expression = SafeExpressionToString(s.KeySelector),
                OrderType = s.OrderType,
                IsThenBy = s.IsThenBy,
            }),
        ];

        List<string>? parameters = queryParameters
            ?.Select(p => JsonSerializer.Serialize(p))
            .ToList();

        return new SpecCacheMetadata(
            entityType: typeof(T).FullName,
            wheres: wheres,
            includes: includes,
            sorts: sorts,
            skip: spec.Skip,
            take: spec.Take,
            isNoTracking: spec.IsNoTracking,
            isSplitQuery: spec.IsSplitQuery,
            specCacheKey: spec.CacheKey,
            selector: null,
            selectorMany: null,
            queryParameters: parameters
        );
    }

    private static SpecCacheMetadata BuildMetadata<T, TResponse>(
        ISpecification<T, TResponse> spec,
        params object[]? queryParameters
    )
        where T : class
        where TResponse : class
    {
        List<WhereMeta> wheres =
        [
            .. spec.Wheres.Select(w => new WhereMeta
            {
                Expression = SafeExpressionToString(w.Filter),
            }),
        ];

        List<IncludeMeta> includes =
        [
            .. spec.Includes.Select(i => new IncludeMeta
            {
                Expression = SafeExpressionToString(i.LamdaExpression),
                PropertyType = i.PropertyType?.FullName,
                EntityType = i.EntityType?.FullName,
                PreviousPropertyType = i.PreviousPropertyType?.FullName,
                InCludeType = i.InCludeType,
            }),
        ];

        List<SortMeta> sorts =
        [
            .. spec.Sorts.Select(s => new SortMeta
            {
                Expression = SafeExpressionToString(s.KeySelector),
                OrderType = s.OrderType,
                IsThenBy = s.IsThenBy,
            }),
        ];

        List<string>? parameters = queryParameters
            ?.Select(p => JsonSerializer.Serialize(p))
            .ToList();

        return new SpecCacheMetadata(
            entityType: typeof(T).FullName,
            wheres: wheres,
            includes: includes,
            sorts: sorts,
            skip: spec.Skip,
            take: spec.Take,
            isNoTracking: spec.IsNoTracking,
            isSplitQuery: spec.IsSplitQuery,
            specCacheKey: spec.CacheKey,
            selector: SafeExpressionToString(spec.Selector),
            selectorMany: SafeExpressionToString(spec.SelectorMany),
            queryParameters: parameters
        );
    }

    private static string SafeExpressionToString(LambdaExpression? expression)
    {
        if (expression is null)
        {
            return string.Empty;
        }

        return expression.ToString();
    }
}
