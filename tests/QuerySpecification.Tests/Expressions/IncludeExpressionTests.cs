namespace Pozitron.QuerySpecification.Tests;

public class IncludeExpressionTests
{
    private readonly Expression<Func<Company, Country>> _expr;

    public IncludeExpressionTests()
    {
        _expr = x => x.Country;
    }

    [Fact]
    public void ThrowsArgumentNullException_GivenNullForLambdaExpression()
    {
        Action sutAction = () => new IncludeExpression(null!, typeof(Company), typeof(Country));

        sutAction.Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void ThrowsArgumentNullException_GivenNullForEntityType()
    {
        Action sutAction = () => new IncludeExpression(_expr, null!, typeof(Country));

        sutAction.Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void ThrowsArgumentNullException_GivenNullForPropertyType()
    {
        Action sutAction = () => new IncludeExpression(_expr, typeof(Company), null!);

        sutAction.Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void ThrowsArgumentNullException_GivenNullForPreviousPropertyType()
    {
        Action sutAction = () => new IncludeExpression(_expr, typeof(Company), typeof(Country), null!);

        sutAction.Should()
            .Throw<ArgumentNullException>();
    }
}
