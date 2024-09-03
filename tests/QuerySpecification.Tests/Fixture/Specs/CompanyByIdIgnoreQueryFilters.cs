namespace Pozitron.QuerySpecification.Tests.Fixture;

public class CompanyByIdIgnoreQueryFilters : Specification<Company>
{
    public CompanyByIdIgnoreQueryFilters(int id)
    {
        Query.Where(company => company.Id == id).IgnoreQueryFilters();
    }
}
