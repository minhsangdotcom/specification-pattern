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
    protected Specification()
    {
        Query = new SpecificationBuilder<T>(this);
    }

    public SpecificationBuilder<T> Query { get; }

    public List<CriteriaInfo<T>> Criteria { get; internal set; } = [];

    public List<IncludeInfo> Includes { get; internal set; } = [];

    public List<OrderByInfo<T>> Sorts { get; internal set; } = [];

    public int Skip { get; internal set; } = -1;

    public int Take { get; internal set; } = -1;

    public bool IsNoTracking { get; internal set; }

    public bool IsSplitQuery { get; internal set; }

    public bool CacheEnabled { get; internal set; }

    public string? CacheKey { get; internal set; }

    public void Update(string key, Expression<Func<T, bool>> newExpr) =>
        Criteria.FirstOrDefault(c => c.Key == key)?.Update(newExpr);

    public void CombineExpression(IEnumerable<CriteriaInfoUpdate<T>> criteriaInfoUpdates)
    {
        const string message = "is null while combing expression.";
        List<CriteriaInfoUpdate<T>> listCriteriaInfoUpdate = [.. criteriaInfoUpdates];
        for (int i = 0; i < Criteria.Count; i++)
        {
            CriteriaInfo<T> criteriaInfo = Criteria[i];
            CriteriaInfoUpdate<T>? criteriaToUpdate = listCriteriaInfoUpdate.Find(x =>
                x.Key == criteriaInfo.Key
            );
            if (criteriaToUpdate == null)
            {
                continue;
            }

            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

            Expression leftExpression = ParameterReplacerVisitor.Replace(
                criteriaInfo.Criteria.Body,
                criteriaInfo.Criteria.Parameters[0],
                parameter
            );

            if (criteriaToUpdate.Criteria == null)
            {
                throw new NullException(
                    nameof(criteriaToUpdate.Criteria),
                    $"{nameof(criteriaToUpdate.Criteria)} {message}",
                    NullType.PropertyOrField,
                    null
                );
            }

            Expression rightExpression = ParameterReplacerVisitor.Replace(
                criteriaToUpdate.Criteria.Body,
                criteriaToUpdate.Criteria.Parameters[0],
                parameter
            );

            BinaryExpression body =
                criteriaToUpdate.Type == BinaryExpressionType.And
                    ? Expression.And(leftExpression, rightExpression)
                    : Expression.Or(leftExpression, rightExpression);

            criteriaInfo.Criteria = Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }

    protected string GetUniqueCachedKey(object? queryParameter = null)
    {
        string query = SpecificationEvaluator.GetStringQuery(this);
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
}
