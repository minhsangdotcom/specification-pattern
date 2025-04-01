using System.Linq.Expressions;

namespace Specification.Models;

public class OrderByInfo<T>
{
    public Expression<Func<T, object>> KeySelector { get; set; } = null!;

    public OrderType OrderType { get; set; }

    public bool IsThenBy { get; set; }
}
