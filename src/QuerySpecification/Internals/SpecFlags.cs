namespace Pozitron.QuerySpecification;

[Flags]
internal enum SpecFlags
{
    IgnoreQueryFilters = 1,
    AsNoTracking = 2,
    AsNoTrackingWithIdentityResolution = 4,
    AsSplitQuery = 8
}
