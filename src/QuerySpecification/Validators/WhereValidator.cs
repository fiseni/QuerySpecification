namespace Pozitron.QuerySpecification;

public class WhereValidator : IValidator
{
    private WhereValidator() { }
    public static WhereValidator Instance = new();

    public bool IsValid<T>(T entity, Specification<T> specification)
    {
        foreach (var whereExpression in specification.WhereExpressions)
        {
            if (whereExpression.FilterFunc(entity) == false) return false;
        }

        return true;
    }
}
