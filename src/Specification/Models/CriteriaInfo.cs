using System.Linq.Expressions;

namespace Specification.Models;

public class CriteriaInfo<T>(string? key, Expression<Func<T, bool>> expr)
    where T : class
{
    public Expression<Func<T, bool>> Criteria { get; set; } =
        expr ?? throw new ArgumentNullException(nameof(expr));

    public string? Key { get; set; } = key;

    public void Update(Expression<Func<T, bool>> newExpr)
    {
        Criteria = newExpr ?? throw new ArgumentNullException(nameof(newExpr));
    }
}
