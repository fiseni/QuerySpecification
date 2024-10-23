namespace Pozitron.QuerySpecification;

public sealed class FlagsEvaluator : IEvaluator
{
    private FlagsEvaluator() { }
    public static FlagsEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        var flags = specification.GetFirstOrDefault<Specification<T>.Flags>();

        if (flags is null) return source;

        if (flags.IgnoreQueryFilters)
        {
            source = source.IgnoreQueryFilters();
        }

        if (flags.AsNoTracking)
        {
            source = source.AsNoTracking();
        }

        if (flags.AsNoTrackingWithIdentityResolution)
        {
            source = source.AsNoTrackingWithIdentityResolution();
        }

        if (flags.AsSplitQuery)
        {
            source = source.AsSplitQuery();
        }

        return source;
    }
}
