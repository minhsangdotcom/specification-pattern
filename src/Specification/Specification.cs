using System.Linq.Expressions;
using System.Text.Json;
using Specification.Builders;
using Specification.Evaluators;
using Specification.Exceptions;
using Specification.Interfaces;
using Specification.Models;

namespace Specification;

public abstract class Specification<T> : ISpecification<T>
    where T : class
{
    public Specification()
    {
        Query = new SpecificationBuilder<T>(this);
    }

    public SpecificationBuilder<T> Query { get; }

    public Expression<Func<T, bool>> Criteria { get; internal set; } = null!;

    public List<IncludeInfo> Includes { get; internal set; } = [];

    public List<OrderByInfo<T>> Sorts { get; internal set; } = [];

    public int Skip { get; internal set; } = -1;

    public int Take { get; internal set; } = -1;

    public bool IsNoTracking { get; internal set; }

    public bool IsSplitQuery { get; internal set; }

    public bool CacheEnabled { get; internal set; }

    public string? CacheKey { get; internal set; }

    internal void CombineExpression(Expression<Func<T, bool>> criteria, BinaryExpressionType type)
    {
        const string message = "is null while combing expression.";

        if (criteria == null)
        {
            throw new NullException(
                nameof(criteria),
                $"{nameof(criteria)} {message}",
                NullType.PropertyOrField,
                null
            );
        }

        if (Criteria == null)
        {
            throw new NullException(
                nameof(Criteria),
                $"{nameof(Criteria)} {message}",
                NullType.PropertyOrField,
                this
            );
        }

        ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

        Expression leftExpression = ParameterReplacerVisitor.Replace(
            Criteria.Body,
            Criteria.Parameters[0],
            parameter
        );

        Expression rightExpression = ParameterReplacerVisitor.Replace(
            criteria.Body,
            criteria.Parameters[0],
            parameter
        );

        BinaryExpression body =
            type == BinaryExpressionType.And
                ? Expression.And(leftExpression, rightExpression)
                : Expression.Or(leftExpression, rightExpression);

        Criteria = Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    protected string GetUniqueCachedKey(object? queryParemeter = null)
    {
        string query = SpecificationEvaluator.SpecStringQuery(this);
        string code = $"{query}";
        if (queryParemeter != null)
        {
            string param = JsonSerializer.Serialize(queryParemeter);
            code += $"~{param}";
        }
        return code;
    }
}

public class Specification<T, TResponse> : Specification<T>, ISpecification<T, TResponse>
    where T : class
    where TResponse : class
{
    public new SpecificationBuilder<T, TResponse> Query => new(this);

    public Expression<Func<T, TResponse>> Selector { get; internal set; } = null!;
}
