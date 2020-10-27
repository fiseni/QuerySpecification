using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class CompanyByIdIncludeStoresThenIncludeAddressSpec : Specification<Company>
    {
        public CompanyByIdIncludeStoresThenIncludeAddressSpec(int id)
        {
            Query.Where(x => x.Id == id)
                .Include(x => x.Stores)
                .ThenInclude(x => x.Address);
        }
    }
}