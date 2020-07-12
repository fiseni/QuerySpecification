﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.UnitTests.Entities
{
    class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CountryId { get; set; }
        public Country Country { get; set; }

        public List<Store> Stores { get; set; } = new List<Store>();
    }
}
