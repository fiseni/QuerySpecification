namespace Pozitron.QuerySpecification;

public static partial class SpecificationBuilderExtensions
{
    public static void Select<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, TResult>> selector)
    {
        builder.Specification.AddOrUpdateInternal(ItemType.Select, selector, (int)SelectType.Select);
    }

    public static void SelectMany<T, TResult>(
        this ISpecificationBuilder<T, TResult> builder,
        Expression<Func<T, IEnumerable<TResult>>> selector)
    {
        builder.Specification.AddOrUpdateInternal(ItemType.Select, selector, (int)SelectType.SelectMany);
    }
}
