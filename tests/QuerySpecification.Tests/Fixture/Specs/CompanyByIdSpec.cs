namespace Pozitron.QuerySpecification.Tests.Fixture;

public class CompanyByIdSpec : Specification<Company>
{
    public CompanyByIdSpec(int id)
    {
        Query.Where(company => company.Id == id);
    }
}
