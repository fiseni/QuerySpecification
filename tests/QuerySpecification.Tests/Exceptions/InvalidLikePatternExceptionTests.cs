namespace Tests.Exceptions;

public class InvalidLikePatternExceptionTests
{
    private const string _defaultMessage = "Invalid like pattern: " + _pattern;
    private const string _pattern = "x";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action sut = () => throw new InvalidLikePatternException(_pattern);

        sut.Should().Throw<InvalidLikePatternException>()
            .WithMessage(_defaultMessage);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        Action sut = () => throw new InvalidLikePatternException(_pattern, inner);

        sut.Should().Throw<InvalidLikePatternException>()
            .WithMessage(_defaultMessage)
            .WithInnerException<Exception>()
            .WithMessage("test");
    }
}
