namespace Pozitron.QuerySpecification.EntityFrameworkCore;

public interface IRepositoryFactory<TRepository>
{
    public TRepository CreateRepository();
}
