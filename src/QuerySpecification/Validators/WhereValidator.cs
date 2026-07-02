namespace Pozitron.QuerySpecification;

/// <summary>
/// Represents a validator for where expressions.
/// </summary>
[ValidatorDiscovery(Order = 10)]
public sealed class WhereValidator : IValidator
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="WhereValidator"/> class.
    /// </summary>
    public static readonly WhereValidator Instance = new();
    private WhereValidator() { }

    /// <inheritdoc/>
    public bool IsValid<T>(T entity, Specification<T> specification)
    {
        // Validation never uses the Order expressions; don't pay their compilation cost.
        var compiledItems = specification.GetCompiledItems(includeOrder: false);

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
