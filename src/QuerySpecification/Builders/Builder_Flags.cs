namespace Pozitron.QuerySpecification;

public static partial class SpecificationBuilderExtensions
{
    public static ISpecificationBuilder<T, TResult> IgnoreQueryFilters<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder) where T : class
        => IgnoreQueryFilters(builder, true);

    public static ISpecificationBuilder<T, TResult> IgnoreQueryFilters<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AddOrUpdateFlag(SpecFlags.IgnoreQueryFilters, true);
        }
        return builder;
    }

    public static ISpecificationBuilder<T> IgnoreQueryFilters<T>(
        this ISpecificationBuilder<T> builder) where T : class
        => IgnoreQueryFilters(builder, true);

    public static ISpecificationBuilder<T> IgnoreQueryFilters<T>(
        this ISpecificationBuilder<T> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AddOrUpdateFlag(SpecFlags.IgnoreQueryFilters, true);
        }
        return builder;
    }

    public static ISpecificationBuilder<T, TResult> AsSplitQuery<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder) where T : class
        => AsSplitQuery(builder, true);

    public static ISpecificationBuilder<T, TResult> AsSplitQuery<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsSplitQuery, true);
        }
        return builder;
    }

    public static ISpecificationBuilder<T> AsSplitQuery<T>(
        this ISpecificationBuilder<T> builder) where T : class
        => AsSplitQuery(builder, true);

    public static ISpecificationBuilder<T> AsSplitQuery<T>(
        this ISpecificationBuilder<T> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsSplitQuery, true);
        }
        return builder;
    }

    public static ISpecificationBuilder<T, TResult> AsNoTracking<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder) where T : class
        => AsNoTracking(builder, true);

    public static ISpecificationBuilder<T, TResult> AsNoTracking<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsTracking, false);
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsNoTrackingWithIdentityResolution, false);
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsNoTracking, true);
        }
        return builder;
    }

    public static ISpecificationBuilder<T> AsNoTracking<T>(
        this ISpecificationBuilder<T> builder) where T : class
        => AsNoTracking(builder, true);

    public static ISpecificationBuilder<T> AsNoTracking<T>(
        this ISpecificationBuilder<T> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsTracking, false);
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsNoTrackingWithIdentityResolution, false);
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsNoTracking, true);
        }
        return builder;
    }

    public static ISpecificationBuilder<T, TResult> AsNoTrackingWithIdentityResolution<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder) where T : class
        => AsNoTrackingWithIdentityResolution(builder, true);

    public static ISpecificationBuilder<T, TResult> AsNoTrackingWithIdentityResolution<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsNoTracking, false);
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsTracking, false);
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsNoTrackingWithIdentityResolution, true);
        }
        return builder;
    }

    public static ISpecificationBuilder<T> AsNoTrackingWithIdentityResolution<T>(
        this ISpecificationBuilder<T> builder) where T : class
        => AsNoTrackingWithIdentityResolution(builder, true);

    public static ISpecificationBuilder<T> AsNoTrackingWithIdentityResolution<T>(
        this ISpecificationBuilder<T> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsNoTracking, false);
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsTracking, false);
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsNoTrackingWithIdentityResolution, true);
        }
        return builder;
    }

    public static ISpecificationBuilder<T, TResult> AsTracking<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder) where T : class
        => AsTracking(builder, true);

    public static ISpecificationBuilder<T, TResult> AsTracking<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsNoTracking, false);
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsNoTrackingWithIdentityResolution, false);
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsTracking, true);
        }
        return builder;
    }

    public static ISpecificationBuilder<T> AsTracking<T>(
        this ISpecificationBuilder<T> builder) where T : class
        => AsTracking(builder, true);

    public static ISpecificationBuilder<T> AsTracking<T>(
        this ISpecificationBuilder<T> builder,
        bool condition) where T : class
    {
        if (condition)
        {
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsNoTracking, false);
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsNoTrackingWithIdentityResolution, false);
            builder.Specification.AddOrUpdateFlag(SpecFlags.AsTracking, true);
        }
        return builder;
    }
}
