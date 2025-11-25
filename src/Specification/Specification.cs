using System.Linq.Expressions;
using System.Text.Json;
using Specification.Builders;
using Specification.Evaluators;
using Specification.Interfaces;
using Specification.Models;

namespace Specification;

public abstract class Specification<T> : ISpecification<T>
    where T : class
{
    protected Specification()
    {
        Query = new SpecificationBuilder<T>(this);
    }

    public SpecificationBuilder<T> Query { get; }

    public List<WhereInfo<T>> Wheres { get; internal set; } = [];

    public List<IncludeInfo> Includes { get; internal set; } = [];

    public List<OrderByInfo<T>> Sorts { get; internal set; } = [];

    public int Skip { get; internal set; } = -1;

    public int Take { get; internal set; } = -1;

    public bool IsNoTracking { get; internal set; }

    public bool IsSplitQuery { get; internal set; }

    public bool CacheEnabled { get; internal set; }

    public string? CacheKey { get; internal set; }

    protected virtual string GetUniqueCachedKey(object? queryParameter = null)
    {
        string query = this.ToStringQuery();
        if (queryParameter == null)
        {
            return query;
        }
        string param = JsonSerializer.Serialize(queryParameter);
        return $"{query}~{param}";
    }
}

public class Specification<T, TResponse> : Specification<T>, ISpecification<T, TResponse>
    where T : class
    where TResponse : class
{
    public new SpecificationBuilder<T, TResponse> Query => new(this);

    public Expression<Func<T, TResponse>> Selector { get; internal set; } = null!;

    protected override string GetUniqueCachedKey(object? queryParameter = null)
    {
        string query = this.ToStringQuery<T, TResponse>();
        if (queryParameter == null)
        {
            return query;
        }
        string param = JsonSerializer.Serialize(queryParameter);
        return $"{query}~{param}";
    }
}
