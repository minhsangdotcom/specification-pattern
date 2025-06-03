using System.Linq.Expressions;

namespace Specification.Models;

public class CriteriaInfoUpdate<T>
    where T : class
{
    public Expression<Func<T, bool>> Criteria { get; set; } = null!;

    public string? Key { get; set; }

    public BinaryExpressionType Type { get; set; }
}
