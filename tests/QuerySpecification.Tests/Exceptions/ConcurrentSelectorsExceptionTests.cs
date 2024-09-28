namespace Tests.Exceptions;

public class ConcurrentSelectorsExceptionTests
{
    private const string _defaultMessage = "Concurrent specification selector transforms defined. Ensure only one of the Select() or SelectMany() transforms is used in the same specification!";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action sut = () => throw new ConcurrentSelectorsException();

        sut.Should().Throw<ConcurrentSelectorsException>()
            .WithMessage(_defaultMessage);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        Action sut = () => throw new ConcurrentSelectorsException(inner);

        sut.Should().Throw<ConcurrentSelectorsException>()
            .WithMessage(_defaultMessage)
            .WithInnerException<Exception>()
            .WithMessage("test");
    }
}
