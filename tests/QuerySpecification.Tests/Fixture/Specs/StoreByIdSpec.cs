using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreByIdSpec : Specification<Store>
    {
        public StoreByIdSpec(int Id)
        {
            Query.Where(x => x.Id == Id);
        }
    }
}
