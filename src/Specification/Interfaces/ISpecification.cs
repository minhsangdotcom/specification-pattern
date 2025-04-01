using System.Linq.Expressions;
using Specification.Builders;
using Specification.Models;

namespace Specification.Interfaces;

public interface ISpecification<T>
    where T : class
{
    SpecificationBuilder<T> Query { get; }

    Expression<Func<T, bool>> Criteria { get; }

    List<IncludeInfo> Includes { get; }

    bool IsNoTracking { get; }

    bool IsSplitQuery { get; }

    bool CacheEnabled { get; }

    string? CacheKey { get; }
}

public interface ISpecification<T, TResponse> : ISpecification<T>
    where T : class
    where TResponse : class
{
    public Expression<Func<T, TResponse>> Selector { get; }

    public Expression<Func<TResponse, bool>> Filter { get; }

    public Expression<Func<TResponse, bool>> Search { get; }

    public List<OrderByInfo<TResponse>> Sorts { get; }

    public int Skip { get; }

    public int Take { get; }
}
