using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Entities
{
    class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int AddressId { get; set; }
        public Address Address { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
