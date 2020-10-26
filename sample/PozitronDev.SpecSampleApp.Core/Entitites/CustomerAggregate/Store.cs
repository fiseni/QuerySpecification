using PozitronDev.Validations;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.SpecSampleApp.Core.Entitites.CustomerAggregate
{
    public class Store
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }

        public int CustomerId { get; private set; }

        public Store(string name, string address)
        {
            name.ValidateFor().NullOrEmpty();

            this.Name = name;
            this.Address = address;
        }
    }
}
