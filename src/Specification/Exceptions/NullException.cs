using System.Net;

namespace Specification.Exceptions;

public class NullException : Exception
{
    public NullException(string name, string message, NullType nullType, object? target)
        : base(WriteMessage(name, message, nullType, target)) { }

    public NullException(string name, string message, NullType nullType, string? targetType)
        : base(WriteMessage(name, message, nullType, targetType)) { }

    public HttpStatusCode HttpStatusCode { get; private set; } = HttpStatusCode.InternalServerError;

    private static string WriteMessage(
        string name,
        string message,
        NullType nullType,
        object? target
    ) =>
        $"{message} ({nullType} {name}"
        + (target != null ? $" of {target.GetType().FullName})" : ") ");

    private static string WriteMessage(
        string name,
        string message,
        NullType nullType,
        string? targetType = null
    ) =>
        $"{message} ({nullType} {name}"
        + (targetType != null ? $" of {targetType.GetType().FullName})" : ") ");
}

public enum NullType
{
    PropertyOrField = 1,
    Object = 2,
}
