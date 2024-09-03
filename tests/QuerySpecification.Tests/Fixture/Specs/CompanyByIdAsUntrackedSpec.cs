namespace Pozitron.QuerySpecification.Tests.Fixture;

public class CompanyByIdAsUntrackedSpec : Specification<Company>
{
    public CompanyByIdAsUntrackedSpec(int id)
    {
        Query.Where(company => company.Id == id).AsNoTracking();
    }
}
