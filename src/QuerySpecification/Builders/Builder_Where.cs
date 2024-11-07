namespace Pozitron.QuerySpecification;

public static partial class SpecificationBuilderExtensions
{
    public static ISpecificationBuilder<T, TResult> Where<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, bool>> criteria)
        => Where(builder, criteria, true);

    public static ISpecificationBuilder<T, TResult> Where<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, bool>> criteria,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.AddInternal(ItemType.Where, criteria);
        }
        return builder;
    }

    public static ISpecificationBuilder<T> Where<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, bool>> criteria)
        => Where(builder, criteria, true);

    public static ISpecificationBuilder<T> Where<T>(
        this ISpecificationBuilder<T> builder,
        Expression<Func<T, bool>> criteria,
        bool condition)
    {
        if (condition)
        {
            builder.Specification.AddInternal(ItemType.Where, criteria);
        }
        return builder;
    }
}
