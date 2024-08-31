using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class CompanyByIdSpec : Specification<Company>, ISingleResultSpecification
    {
        public CompanyByIdSpec(int id)
        {
            Query.Where(company => company.Id == id);
        }
    }
}
