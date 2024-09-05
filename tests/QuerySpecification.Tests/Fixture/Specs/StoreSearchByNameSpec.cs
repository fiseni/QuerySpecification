namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreSearchByNameSpec : Specification<Store>
{
    public StoreSearchByNameSpec(string searchTerm)
    {
        Query.Like(x => x.Name!, "%" + searchTerm + "%");
    }
}
