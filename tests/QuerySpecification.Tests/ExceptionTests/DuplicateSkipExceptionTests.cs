﻿using FluentAssertions;
using Xunit;

namespace Pozitron.QuerySpecification.Tests;

public class DuplicateSkipExceptionTests
{
    private const string defaultMessage = "Duplicate use of the Skip(). Ensure you don't use both Paginate() and Skip() in the same specification!";

    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        Action action = () => throw new DuplicateSkipException();

        action.Should().Throw<DuplicateSkipException>().WithMessage(defaultMessage);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        Exception inner = new Exception("test");
        Action action = () => throw new DuplicateSkipException(inner);

        action.Should().Throw<DuplicateSkipException>().WithMessage(defaultMessage).WithInnerException<Exception>().WithMessage("test");
    }
}
