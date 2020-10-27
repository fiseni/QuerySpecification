using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3.IntegrationTests.Fixture.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string? Street { get; set; }

        public int StoreId { get; set; }
        public Store? Store { get; set; }

        public object GetSomethingFromAddress()
        {
            return new object();
        }
    }
}
