using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreSearchByNameSpec : Specification<Store>
{
    public StoreSearchByNameSpec(string searchTerm)
    {
        Query.Search(x => x.Name!, "%" + searchTerm + "%");
    }
}
