namespace Pozitron.QuerySpecification;

public interface IIncludableSpecificationBuilder<T, TResult, out TProperty> : ISpecificationBuilder<T, TResult> where T : class
{
}

public interface IIncludableSpecificationBuilder<T, out TProperty> : ISpecificationBuilder<T> where T : class
{
}

internal class IncludableSpecificationBuilder<T, TResult, TProperty>(Specification<T, TResult> specification)
    : SpecificationBuilder<T, TResult>(specification), IIncludableSpecificationBuilder<T, TResult, TProperty> where T : class
{
}

internal class IncludableSpecificationBuilder<T, TProperty>(Specification<T> specification)
    : SpecificationBuilder<T>(specification), IIncludableSpecificationBuilder<T, TProperty> where T : class
{
}
