using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class CompanyByIdSpec : Specification<Company>
    {
        public CompanyByIdSpec(int id)
        {
            Query.Where(company => company.Id == id);
        }
    }
}
