namespace Pozitron.QuerySpecification;

[Obsolete("Use ISingleResultSpecification<T> instead. This interface will be removed in a future version of Pozitron.QuerySpecification.")]
public interface ISingleResultSpecification
{
}

public interface ISingleResultSpecification<T> : ISpecification<T>
{
}

public interface ISingleResultSpecification<T, TResult> : ISpecification<T, TResult>
{
}
