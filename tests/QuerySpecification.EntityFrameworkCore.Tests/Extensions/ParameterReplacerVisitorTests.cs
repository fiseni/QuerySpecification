using System.Linq.Expressions;

namespace Tests.Extensions;

public class ParameterReplacerVisitorTests
{
    [Fact]
    public void Replace_ReturnsExpressionWithReplacedParameter()
    {
        Expression<Func<int, decimal, bool>> expected = (y, z) => y == 1;

        Expression<Func<int, decimal, bool>> expression = (x, z) => x == 1;
        var oldParameter = expression.Parameters[0];
        var newExpression = Expression.Parameter(typeof(int), "y");

        var result = ParameterReplacerVisitor.Replace(expression, oldParameter, newExpression);

        result.ToString().Should().Be(expected.ToString());
    }
}
