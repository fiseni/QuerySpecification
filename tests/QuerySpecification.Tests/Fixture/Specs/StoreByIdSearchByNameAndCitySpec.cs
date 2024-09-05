namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreByIdSearchByNameAndCitySpec : Specification<Store>
{
    public StoreByIdSearchByNameAndCitySpec(int id, string searchTerm)
    {
        Query.Where(x => x.Id == id)
            .Like(x => x.Name!, "%" + searchTerm + "%", 1)
            .Like(x => x.City!, "%" + searchTerm + "%", 2);
    }
}
