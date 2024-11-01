namespace Pozitron.QuerySpecification;

public sealed class WhereValidator : IValidator
{
    private WhereValidator() { }
    public static WhereValidator Instance = new();

    public bool IsValid<T>(T entity, Specification<T> specification)
    {
        if (specification.IsEmpty) return true;

        var compiledStates = specification.GetCompiledStates();

        foreach (var state in compiledStates)
        {
            if (state.Type == StateType.Where && state.Reference is Func<T, bool> compiledExpr)
            {
                if (compiledExpr(entity) == false)
                    return false;
            }
        }

        return true;
    }
}
