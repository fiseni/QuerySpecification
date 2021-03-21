using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class CompanyByIdAsUntrackedWithIdentityResolutionSpec : Specification<Company>, ISingleResultSpecification
    {
        public CompanyByIdAsUntrackedWithIdentityResolutionSpec(int id)
        {
            Query.Where(company => company.Id == id).AsNoTrackingWithIdentityResolution();
        }
    }
}
