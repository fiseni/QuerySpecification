namespace Tests.Exceptions;

public class EntityNotFoundExceptionTests
{
    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        var message = "The queried entity was not found!";
        Action sut = () => throw new EntityNotFoundException();

        sut.Should().Throw<EntityNotFoundException>()
            .WithMessage(message);
    }

    [Fact]
    public void ThrowWithParameterConstructor()
    {
        var entityName = "test";
        var message = $"The queried entity: test was not found!";

        Action sut = () => throw new EntityNotFoundException(entityName);

        sut.Should().Throw<EntityNotFoundException>()
            .WithMessage(message);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        var entityName = "test";
        var message = $"The queried entity: test was not found!";

        Action sut = () => throw new EntityNotFoundException(entityName, inner);

        sut.Should().Throw<EntityNotFoundException>()
            .WithMessage(message)
            .WithInnerException<Exception>()
            .WithMessage("test");
    }
}
