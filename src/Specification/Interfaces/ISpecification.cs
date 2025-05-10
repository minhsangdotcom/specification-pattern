using System.Linq.Expressions;
using Specification.Builders;
using Specification.Models;

namespace Specification.Interfaces;

public interface ISpec<T>
    where T : class
{
    List<CriteriaInfo<T>> Criteria { get; }

    List<IncludeInfo> Includes { get; }

    public List<OrderByInfo<T>> Sorts { get; }

    public int Skip { get; }

    public int Take { get; }

    bool IsNoTracking { get; }

    bool IsSplitQuery { get; }

    bool CacheEnabled { get; }

    string? CacheKey { get; }
}

public interface ISpecification<T> : ISpec<T>
    where T : class
{
    SpecificationBuilder<T> Query { get; }
}

public interface ISpecification<T, TResponse> : ISpec<T>
    where T : class
    where TResponse : class
{
    SpecificationBuilder<T, TResponse> Query { get; }

    public Expression<Func<T, TResponse>> Selector { get; }
}
