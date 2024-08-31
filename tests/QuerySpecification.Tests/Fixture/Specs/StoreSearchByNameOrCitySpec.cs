using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreSearchByNameOrCitySpec : Specification<Store>
{
    public StoreSearchByNameOrCitySpec(string searchTerm)
    {
        Query.Search(x => x.Name!, "%" + searchTerm + "%")
            .Search(x => x.City!, "%" + searchTerm + "%");
    }
}
