using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreByIdSpec : Specification<Store>
{
    public StoreByIdSpec(int Id)
    {
        Query.Where(x => x.Id == Id);
    }
}
