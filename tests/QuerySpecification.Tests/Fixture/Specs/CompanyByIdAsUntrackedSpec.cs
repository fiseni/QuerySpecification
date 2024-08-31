using Pozitron.QuerySpecification.Tests.Fixture.Entities;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs;

public class CompanyByIdAsUntrackedSpec : Specification<Company>, ISingleResultSpecification
{
    public CompanyByIdAsUntrackedSpec(int id)
    {
        Query.Where(company => company.Id == id).AsNoTracking();
    }
}
