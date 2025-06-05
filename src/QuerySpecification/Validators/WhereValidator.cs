namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents a validator for where expressions.
/// </summary>
[ValidatorDiscovery(Order = -100)]
public sealed class WhereValidator : IValidator
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="WhereValidator"/> class.
    /// </summary>
    public static WhereValidator Instance = new();
    private WhereValidator() { }

    /// <inheritdoc/>
    public bool IsValid<T>(T entity, Specification<T> specification)
    {
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
