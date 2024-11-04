namespace Pozitron.QuerySpecification;

public sealed class WhereValidator : IValidator
{
    private WhereValidator() { }
    public static WhereValidator Instance = new();

    public bool IsValid<T>(T entity, Specification<T> specification)
    {
        if (specification.IsEmpty) return true;

        var compiledItems = specification.GetCompiledItems();

        foreach (var item in compiledItems)
        {
            if (item.Type == ItemType.Where && item.Reference is Func<T, bool> compiledExpr)
            {
                if (compiledExpr(entity) == false)
                    return false;
            }
        }

        return true;
    }
}
