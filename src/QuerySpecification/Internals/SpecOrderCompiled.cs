using System.Collections.Concurrent;
using System.Reflection;

namespace Pozitron.QuerySpecification;

/*
    Compiled form of an Order expression, used by the in-memory evaluator.
    Order expressions are stored as Expression<Func<T, object?>>. Compiling that directly boxes every value-type
    key during sorting and compares through Comparer<object> (interface dispatch + unboxing per comparison).
    When the body is a boxing Convert node, we strip it and compile a typed key selector instead, so sorting
    runs allocation-free with Comparer<TKey>. The sort is dispatched through the typed subclass.
 */
internal abstract class SpecOrderCompiled<T>
{
    // The object-returning delegate, exposed through the public OrderExpressionsCompiled property.
    public abstract Func<T, object?> KeySelector { get; }

    public abstract IOrderedEnumerable<T> ApplyOrderBy(IEnumerable<T> source, bool descending);
    public abstract IOrderedEnumerable<T> ApplyThenBy(IOrderedEnumerable<T> source, bool descending);

    // One factory per key type (per T); built once via reflection, then reused for all specs.
    private static readonly ConcurrentDictionary<Type, Func<Expression<Func<T, object?>>, SpecOrderCompiled<T>>> _typedFactories = new();

    public static SpecOrderCompiled<T> Create(Expression<Func<T, object?>> expression)
    {
        if (expression.Body is UnaryExpression { NodeType: ExpressionType.Convert, Method: null } convert
            && convert.Type == typeof(object)
            && convert.Operand.Type.IsValueType)
        {
            var factory = _typedFactories.GetOrAdd(convert.Operand.Type, CreateTypedFactory);
            return factory(expression);
        }

        // Reference-type keys don't box; compiling the expression as-is is optimal.
        return new SpecOrderCompiledBoxed<T>(expression.Compile());
    }

    private static Func<Expression<Func<T, object?>>, SpecOrderCompiled<T>> CreateTypedFactory(Type keyType)
        => typeof(SpecOrderCompiled<T>)
            .GetMethod(nameof(CreateTyped), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(keyType)
            .CreateDelegate<Func<Expression<Func<T, object?>>, SpecOrderCompiled<T>>>();

    private static SpecOrderCompiled<T> CreateTyped<TKey>(Expression<Func<T, object?>> expression)
    {
        var operand = ((UnaryExpression)expression.Body).Operand;
        var typedLambda = Expression.Lambda<Func<T, TKey>>(operand, expression.Parameters);
        return new SpecOrderCompiledTyped<T, TKey>(typedLambda.Compile(), expression);
    }
}

internal sealed class SpecOrderCompiledBoxed<T>(Func<T, object?> keySelector) : SpecOrderCompiled<T>
{
    public override Func<T, object?> KeySelector => keySelector;

    public override IOrderedEnumerable<T> ApplyOrderBy(IEnumerable<T> source, bool descending)
        => descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);

    public override IOrderedEnumerable<T> ApplyThenBy(IOrderedEnumerable<T> source, bool descending)
        => descending ? source.ThenByDescending(keySelector) : source.ThenBy(keySelector);
}

internal sealed class SpecOrderCompiledTyped<T, TKey>(Func<T, TKey> keySelector, Expression<Func<T, object?>> sourceExpression)
    : SpecOrderCompiled<T>
{
    // Compiled only if the public OrderExpressionsCompiled property is read; sorting uses the typed delegate.
    // A benign race may compile it twice; both results are equivalent.
    private Func<T, object?>? _boxedKeySelector;

    public override Func<T, object?> KeySelector => _boxedKeySelector ??= sourceExpression.Compile();

    public override IOrderedEnumerable<T> ApplyOrderBy(IEnumerable<T> source, bool descending)
        => descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);

    public override IOrderedEnumerable<T> ApplyThenBy(IOrderedEnumerable<T> source, bool descending)
        => descending ? source.ThenByDescending(keySelector) : source.ThenBy(keySelector);
}
