﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Fixture.Entities
{
    public class Store
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }

        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        public int AddressId { get; set; }
        public Address? Address { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();

        public object GetSomethingFromStore()
        {
            return new object();
        }
    }
}
