using Specification.Interfaces;

namespace Specification.Builders;

public class SpecificationBuilder<T>(Specification<T>? Spec) : ISpecificationBuilder<T>
    where T : class
{
    public Specification<T>? Spec { get; } = Spec;
}

//inlcude
public class IncludableSpecificationBuilder<T, TProperty>(Specification<T> Spec)
    : IIncludableSpecificationBuilder<T, TProperty>
    where T : class
{
    public Specification<T>? Spec { get; } = Spec;
}

public class SpecificationBuilder<T, TResponse>(Specification<T, TResponse>? Spec)
    : ISpecificationBuilder<T, TResponse>
    where T : class
    where TResponse : class
{
    public Specification<T, TResponse>? Spec { get; } = Spec;
}

// include with select
public class IncludableSpecificationBuilder<T, TResponse, TProperty>(
    Specification<T, TResponse> Spec
) : IIncludableSpecificationBuilder<T, TResponse, TProperty>
    where T : class
    where TResponse : class
{
    public Specification<T, TResponse>? Spec { get; } = Spec;
}
