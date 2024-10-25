namespace Pozitron.QuerySpecification;

public sealed class FlagsEvaluator : IEvaluator
{
    private FlagsEvaluator() { }
    public static FlagsEvaluator Instance = new();

    public IQueryable<T> Evaluate<T>(IQueryable<T> source, Specification<T> specification) where T : class
    {
        var flags = specification.GetEfFlag();

        if (flags is null) return source;

        if ((flags & EfFlag.IgnoreQueryFilters) == EfFlag.IgnoreQueryFilters)
        {
            source = source.IgnoreQueryFilters();
        }

        if ((flags & EfFlag.AsNoTracking) == EfFlag.AsNoTracking)
        {
            source = source.AsNoTracking();
        }

        if ((flags & EfFlag.AsNoTrackingWithIdentityResolution) == EfFlag.AsNoTrackingWithIdentityResolution)
        {
            source = source.AsNoTrackingWithIdentityResolution();
        }

        if ((flags & EfFlag.AsSplitQuery) == EfFlag.AsSplitQuery)
        {
            source = source.AsSplitQuery();
        }

        return source;
    }
}
