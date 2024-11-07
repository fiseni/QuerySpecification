namespace Pozitron.QuerySpecification;

public static partial class SpecificationBuilderExtensions
{
    public static ISpecificationBuilder<T, TResult> Take<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int take)
        => Take(builder, take, true);

    public static ISpecificationBuilder<T, TResult> Take<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int take,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.GetOrCreate<SpecPaging>(ItemType.Paging).Take = take;
        }
        return builder;
    }

    public static ISpecificationBuilder<T> Take<T>(
        this ISpecificationBuilder<T> builder,
        int take)
        => Take(builder, take, true);

    public static ISpecificationBuilder<T> Take<T>(
        this ISpecificationBuilder<T> builder,
        int take,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.GetOrCreate<SpecPaging>(ItemType.Paging).Take = take;
        }
        return builder;
    }

    public static ISpecificationBuilder<T, TResult> Skip<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int skip)
        => Skip(builder, skip, true);

    public static ISpecificationBuilder<T, TResult> Skip<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        int skip,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.GetOrCreate<SpecPaging>(ItemType.Paging).Skip = skip;
        }
        return builder;
    }

    public static ISpecificationBuilder<T> Skip<T>(
        this ISpecificationBuilder<T> builder,
        int skip)
        => Skip(builder, skip, true);

    public static ISpecificationBuilder<T> Skip<T>(
        this ISpecificationBuilder<T> builder,
        int skip,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.GetOrCreate<SpecPaging>(ItemType.Paging).Skip = skip;
        }
        return builder;
    }
}
