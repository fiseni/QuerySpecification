namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreSearchByNameAndCitySpec : Specification<Store>
{
    public StoreSearchByNameAndCitySpec(string searchTerm)
    {
        Query.Like(x => x.Name!, "%" + searchTerm + "%", 1)
            .Like(x => x.City!, "%" + searchTerm + "%", 2);
    }
}
