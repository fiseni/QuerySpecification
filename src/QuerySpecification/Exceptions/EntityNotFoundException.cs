namespace Pozitron.QuerySpecification;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException()
        : base($"The queried entity was not found!")
    {
    }

    public EntityNotFoundException(string entityName)
        : base($"The queried entity: {entityName} was not found!")
    {
    }

    public EntityNotFoundException(string entityName, Exception innerException)
        : base($"The queried entity: {entityName} was not found!", innerException)
    {
    }
}
