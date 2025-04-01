namespace Specification.Interfaces;

//builder design pattern
public interface ISpecificationBuilder<T>
    where T : class
{
    Specification<T>? Spec { get; }
}

public interface ISpecificationBuilder<T, TResponse>
    where T : class
    where TResponse : class
{
    Specification<T, TResponse>? Spec { get; }
}

// include
public interface IIncludableSpecificationBuilder<T, TProperty> : ISpecificationBuilder<T>
    where T : class;

public interface IIncludableSpecificationBuilder<T, TResponse, TProperty>
    : ISpecificationBuilder<T, TResponse>
    where T : class
    where TResponse : class;
