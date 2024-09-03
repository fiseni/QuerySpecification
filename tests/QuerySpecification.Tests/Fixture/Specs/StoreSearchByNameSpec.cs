namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreSearchByNameSpec : Specification<Store>
{
    public StoreSearchByNameSpec(string searchTerm)
    {
        Query.Search(x => x.Name!, "%" + searchTerm + "%");
    }
}
