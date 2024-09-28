﻿namespace Tests.Exceptions;

public class SelectorNotFoundExceptionTests
{
    private const string _defaultMessage = "The specification must have a selector transform defined. Ensure either Select() or SelectMany() is used in the specification!";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action sut = () => throw new SelectorNotFoundException();

        sut.Should().Throw<SelectorNotFoundException>()
            .WithMessage(_defaultMessage);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        Action sut = () => throw new SelectorNotFoundException(inner);

        sut.Should().Throw<SelectorNotFoundException>()
            .WithMessage(_defaultMessage)
            .WithInnerException<Exception>()
            .WithMessage("test");
    }
}
