﻿namespace Pozitron.QuerySpecification;

public interface IOrderedSpecificationBuilder<T, TResult> : ISpecificationBuilder<T, TResult>
{
}

public interface IOrderedSpecificationBuilder<T> : ISpecificationBuilder<T>
{
}
public interface ISpecificationBuilder<T, TResult>
{
    internal Specification<T, TResult> Specification { get; }
}

public interface ISpecificationBuilder<T>
{
    internal Specification<T> Specification { get; }
}

internal class SpecificationBuilder<T, TResult> : IOrderedSpecificationBuilder<T, TResult>, ISpecificationBuilder<T, TResult>
{
    public Specification<T, TResult> Specification { get; }

    public SpecificationBuilder(Specification<T, TResult> specification)
    {
        Specification = specification;
    }
}

internal class SpecificationBuilder<T> : IOrderedSpecificationBuilder<T>, ISpecificationBuilder<T>
{
    public Specification<T> Specification { get; }

    public SpecificationBuilder(Specification<T> specification)
    {
        Specification = specification;
    }
}
