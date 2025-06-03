using System.Linq.Expressions;
using Specification.Exceptions;

namespace Specification.Models;

public class CriteriaInfo<T>(string? key, Expression<Func<T, bool>> expr)
    where T : class
{
    private const string Message = "is null while combing expression.";
    public Expression<Func<T, bool>> Criteria { get; set; } =
        expr
        ?? throw new NullException(
            nameof(Criteria),
            $"{nameof(Criteria)} {Message}",
            NullType.PropertyOrField,
            typeof(T).FullName
        );

    public string? Key { get; set; } = key;

    public void Update(Expression<Func<T, bool>> newExpr)
    {
        Criteria =
            newExpr
            ?? throw new NullException(
                nameof(newExpr),
                $"{nameof(newExpr)} {Message}",
                NullType.PropertyOrField,
                null
            );
    }
}
