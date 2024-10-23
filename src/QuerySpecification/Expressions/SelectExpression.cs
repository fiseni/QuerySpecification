using System.Diagnostics.CodeAnalysis;

namespace Pozitron.QuerySpecification;

public sealed class SelectExpression<T, TResult>
{
    public Expression<Func<T, TResult>>? Selector { get; internal set; }
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany { get; internal set; }
}

public static class SelectExpressionExtensions
{
    public static SelectExpression<T, TResult> Validate<T, TResult>([NotNull] this SelectExpression<T, TResult>? expression)
    {
        if (expression is null) 
            throw new SelectorNotFoundException();

        if (expression.Selector is null && expression.SelectorMany is null) 
            throw new SelectorNotFoundException();

        if (expression.Selector is not null && expression.SelectorMany is not null) 
            throw new ConcurrentSelectorsException();

        return expression;
    }
}
