using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class CompanyByIdAsUntrackedSpec : Specification<Company>
    {
        public CompanyByIdAsUntrackedSpec(int id)
        {
            Query.Where(company => company.Id == id).AsNoTracking();
        }
    }
}
