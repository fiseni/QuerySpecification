namespace Pozitron.QuerySpecification;

public class WhereValidator : IValidator
{
    private WhereValidator() { }
    public static WhereValidator Instance { get; } = new WhereValidator();

    public bool IsValid<T>(T entity, Specification<T> specification)
    {
        foreach (var info in specification.Context.WhereExpressions)
        {
            if (info.FilterFunc(entity) == false) return false;
        }

        return true;
    }
}
