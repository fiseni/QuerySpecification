using Pozitron.QuerySpecification.Tests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification.Tests.Fixture.Specs
{
    public class CompanyByIdAsUntrackedWithIdentityResolutionSpec : Specification<Company>, ISingleResultSpecification
    {
        public CompanyByIdAsUntrackedWithIdentityResolutionSpec(int id)
        {
            Query.Where(company => company.Id == id).AsNoTrackingWithIdentityResolution();
        }
    }
}
