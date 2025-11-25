using System.Linq.Expressions;

namespace Specification.Models;

public class WhereInfo<T>(Expression<Func<T, bool>> filter)
    where T : class
{
    public Expression<Func<T, bool>> Filter { get; } = filter;
}
