namespace Pozitron.QuerySpecification;

public class SelectorNotFoundException : Exception
{
    private const string message = "The specification must have a selector transform defined. Ensure either Select() or SelectMany() is used in the specification!";

    public SelectorNotFoundException()
        : base(message)
    {
    }

    public SelectorNotFoundException(Exception innerException)
        : base(message, innerException)
    {
    }
}
