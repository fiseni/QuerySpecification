namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreByIdSpec : Specification<Store>
{
    public StoreByIdSpec(int id)
    {
        Query.Where(x => x.Id == id);
    }
}
