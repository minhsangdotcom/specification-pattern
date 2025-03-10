using System.Net;

namespace Specification.Exceptions;

public class NullException(NullExceptionParameters parameters) : Exception(WriteMessage(parameters))
{
    public HttpStatusCode HttpStatusCode { get; private set; }

    private static string WriteMessage(NullExceptionParameters parameters) =>
        $"{parameters.Message} ({parameters.NullType} {parameters.Name}"
        + (parameters.Target != null ? $" of {parameters.Target.GetType().FullName})" : ") ");
}

public record NullExceptionParameters(
    string Name,
    string Message,
    NullType NullType,
    object? Target
);

public enum NullType
{
    PropertyOrField = 1,
    Object = 2,
}
