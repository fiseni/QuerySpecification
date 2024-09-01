namespace Pozitron.QuerySpecification;

public class WhereValidator : IValidator
{
    private WhereValidator() { }
    public static WhereValidator Instance = new();

    public bool IsValid<T>(T entity, Specification<T> specification)
    {
        foreach (var info in specification.WhereExpressions)
        {
            if (info.FilterFunc(entity) == false) return false;
        }

        return true;
    }
}
