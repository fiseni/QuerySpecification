namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreSearchByNameOrCitySpec : Specification<Store>
{
    public StoreSearchByNameOrCitySpec(string searchTerm)
    {
        Query.Like(x => x.Name!, "%" + searchTerm + "%")
            .Like(x => x.City!, "%" + searchTerm + "%");
    }
}
