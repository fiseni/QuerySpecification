using PozitronDev.QuerySpecification.UnitTests.Fixture.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Specs
{
    public class StoreByIdAndNameSpec : Specification<Store>
    {
        public StoreByIdAndNameSpec(int Id, string name)
        {
            Query.Where(x => x.Id == Id)
                .Where(x => x.Name == name);
        }
    }
}
