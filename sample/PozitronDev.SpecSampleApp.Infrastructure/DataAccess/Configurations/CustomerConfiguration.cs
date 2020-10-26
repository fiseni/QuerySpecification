using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PozitronDev.SpecSampleApp.Core.Entitites.CustomerAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.SpecSampleApp.Infrastructure.DataAccess.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(nameof(Customer));

            builder.Metadata.FindNavigation(nameof(Customer.Stores))
                            .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasKey(x => x.Id);
        }
    }
}
