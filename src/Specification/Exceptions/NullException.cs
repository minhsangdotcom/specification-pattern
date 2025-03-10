using System.Net;

namespace Specification.Exceptions;

public class NullException(string name, string message, NullType nullType, object? target)
    : Exception(WriteMessage(name, message, nullType, target))
{
    public HttpStatusCode HttpStatusCode { get; private set; }

    private static string WriteMessage(
        string name,
        string message,
        NullType nullType,
        object? target
    ) =>
        $"{message} ({nullType} {name}"
        + (target != null ? $" of {target.GetType().FullName})" : ") ");
}

public enum NullType
{
    PropertyOrField = 1,
    Object = 2,
}
