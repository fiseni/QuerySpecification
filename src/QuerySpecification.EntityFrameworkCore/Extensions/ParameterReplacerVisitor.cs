namespace Pozitron.QuerySpecification;

internal class ParameterReplacerVisitor : ExpressionVisitor
{
    private readonly ParameterExpression _oldParameter;
    private readonly Expression _newExpression;

    private ParameterReplacerVisitor(ParameterExpression oldParameter, Expression newExpression)
    {
        _oldParameter = oldParameter;
        _newExpression = newExpression;
    }

    internal static Expression Replace(Expression expression, ParameterExpression oldParameter, Expression newExpression)
      => new ParameterReplacerVisitor(oldParameter, newExpression).Visit(expression);

    protected override Expression VisitParameter(ParameterExpression node)
      => node == _oldParameter ? _newExpression : node;
}
