using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class StoreByIdAndNameSpec : Specification<Store>
{
    public StoreByIdAndNameSpec(int Id, string name)
    {
        Query.Where(x => x.Id == Id)
            .Where(x => x.Name == name);
    }
}
