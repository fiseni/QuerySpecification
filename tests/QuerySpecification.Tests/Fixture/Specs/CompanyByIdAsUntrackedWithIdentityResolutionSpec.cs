namespace Pozitron.QuerySpecification.Tests.Fixture;

public class CompanyByIdAsUntrackedWithIdentityResolutionSpec : Specification<Company>
{
    public CompanyByIdAsUntrackedWithIdentityResolutionSpec(int id)
    {
        Query.Where(company => company.Id == id).AsNoTrackingWithIdentityResolution();
    }
}
