namespace Pozitron.QuerySpecification;

/// <summary>
/// Evaluator that applies all EF flag operations (tracking behavior, split query, ignore filters/auto-includes)
/// by reading the flags in a single pass. It replaces the individual flag evaluators in the default pipeline.
/// </summary>
/// <remarks>
/// It is excluded from auto-discovery (<see cref="DiscoveryAttribute.Enable"/> is false) so that, when auto-discovery
/// is enabled, the individual flag evaluators are used instead and this one is not applied twice.
/// </remarks>
[EvaluatorDiscovery(Enable = false)]
public sealed class FlagsEvaluator : IEvaluator, ISpecItemKind
{
    /// <summary>
    /// Gets the singleton instance of the <see cref="FlagsEvaluator"/> class.
    /// </summary>
    public static FlagsEvaluator Instance = new();
    private FlagsEvaluator() { }

    int ISpecItemKind.ItemType => Pozitron.QuerySpecification.ItemType.Flags;

    /// <inheritdoc/>
    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        var flags = specification.Flags;
        if (flags == 0) return source;

        // Applied in the same relative order as the individual evaluators (Order 70..120) to preserve the generated expression.
        if ((flags & SpecFlags.IgnoreAutoIncludes) != 0) source = source.IgnoreAutoIncludes();
        if ((flags & SpecFlags.IgnoreQueryFilters) != 0) source = source.IgnoreQueryFilters();
        if ((flags & SpecFlags.AsSplitQuery) != 0) source = source.AsSplitQuery();
        if ((flags & SpecFlags.AsNoTracking) != 0) source = source.AsNoTracking();
        if ((flags & SpecFlags.AsNoTrackingWithIdentityResolution) != 0) source = source.AsNoTrackingWithIdentityResolution();
        if ((flags & SpecFlags.AsTracking) != 0) source = source.AsTracking();

        return source;
    }
}
