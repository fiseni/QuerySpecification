using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PozitronDev.SpecSampleApp.Core.Entitites.CustomerAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.SpecSampleApp.Infrastructure.DataAccess.Configurations
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.ToTable(nameof(Store));

            builder.HasKey(x => x.Id);
        }
    }
}
