namespace Tests.Exceptions;

public class SpecAutoDiscoveryExceptionTests
{
    private const string _defaultMessage = "The auto-discovery of evaluators failed while scanning the loaded assemblies. Disable the feature and contact the authors!";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action sut = () => throw new SpecAutoDiscoveryException();

        sut.Should().Throw<SpecAutoDiscoveryException>()
            .WithMessage(_defaultMessage);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        Action sut = () => throw new SpecAutoDiscoveryException(inner);

        sut.Should().Throw<SpecAutoDiscoveryException>()
            .WithMessage(_defaultMessage)
            .WithInnerException<Exception>()
            .WithMessage("test");
    }
}
