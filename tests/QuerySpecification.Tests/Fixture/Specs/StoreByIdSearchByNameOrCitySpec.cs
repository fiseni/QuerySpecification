namespace Pozitron.QuerySpecification.Tests.Fixture;

public class StoreByIdSearchByNameOrCitySpec : Specification<Store>
{
    public StoreByIdSearchByNameOrCitySpec(int id, string searchTerm)
    {
        Query.Where(x => x.Id == id)
            .Like(x => x.Name!, "%" + searchTerm + "%")
            .Like(x => x.City!, "%" + searchTerm + "%");
    }
}
