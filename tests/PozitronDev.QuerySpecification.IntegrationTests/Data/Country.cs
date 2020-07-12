﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.IntegrationTests.Data
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Company> Companies { get; set; } = new List<Company>();
    }
}
