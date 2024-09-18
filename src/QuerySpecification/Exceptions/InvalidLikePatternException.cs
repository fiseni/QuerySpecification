namespace Pozitron.QuerySpecification;

public class InvalidLikePatternException : Exception
{
    private const string _message = "Invalid like pattern: ";

    public InvalidLikePatternException(string pattern)
        : base($"{_message}{pattern}")
    {
    }

    public InvalidLikePatternException(string pattern, Exception innerException)
        : base($"{_message}{pattern}", innerException)
    {
    }
}
