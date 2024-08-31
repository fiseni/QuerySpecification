using System.Linq.Expressions;

namespace Pozitron.QuerySpecification;

internal class ParameterReplacerVisitor : ExpressionVisitor
{
    private Expression _newExpression;
    private ParameterExpression _oldParameter;

    private ParameterReplacerVisitor(ParameterExpression oldParameter, Expression newExpression)
    {
        _oldParameter = oldParameter;
        _newExpression = newExpression;
    }

    internal static Expression Replace(Expression expression, ParameterExpression oldParameter, Expression newExpression)
    {
        return new ParameterReplacerVisitor(oldParameter, newExpression).Visit(expression);
    }

    protected override Expression VisitParameter(ParameterExpression p)
    {
        if (p == _oldParameter)
        {
            return _newExpression;
        }
        else
        {
            return p;
        }
    }
}
