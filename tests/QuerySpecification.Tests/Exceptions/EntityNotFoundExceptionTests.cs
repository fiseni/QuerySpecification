namespace Tests.Exceptions;

public class EntityNotFoundExceptionTests
{
    [Fact]
    public void ThrowWithDefaultConstructor()
    {
        var message = "The queried entity was not found!";
        Action action = () => throw new EntityNotFoundException();

        action.Should().Throw<EntityNotFoundException>()
            .WithMessage(message);
    }

    [Fact]
    public void ThrowWithParameterConstructor()
    {
        var entityName = "test";
        var message = $"The queried entity: test was not found!";
        
        Action action = () => throw new EntityNotFoundException(entityName);

        action.Should().Throw<EntityNotFoundException>()
            .WithMessage(message);
    }

    [Fact]
    public void ThrowWithInnerException()
    {
        var inner = new Exception("test");
        var entityName = "test";
        var message = $"The queried entity: test was not found!";

        Action action = () => throw new EntityNotFoundException(entityName, inner);

        action.Should().Throw<EntityNotFoundException>()
            .WithMessage(message)
            .WithInnerException<Exception>()
            .WithMessage("test");
    }
}
