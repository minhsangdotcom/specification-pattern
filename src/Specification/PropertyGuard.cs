using Ardalis.GuardClauses;
using Specification.Exceptions;

namespace Specification;

public static class PropertyGuard
{
    public static T Null<T>(
        this IGuardClause guardClause,
        T? property,
        string propertyName,
        string message,
        object? target = null
    ) =>
        property
        ?? throw new NullException(
            new NullExceptionParameters(propertyName, message, NullType.PropertyOrField, target)
        );
}
