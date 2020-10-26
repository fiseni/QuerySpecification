using PozitronDev.QuerySpecification.EntityFrameworkCore3;
using PozitronDev.SpecSampleApp.Core.Interfaces;
using PozitronDev.SpecSampleApp.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PozitronDev.SpecSampleApp.Infrastructure.Data
{
    public class MyRepository<T> : RepositoryBase<T>, IRepository<T> where T : class, IAggregateRoot
    {
        private readonly SampleDbContext dbContext;

        public MyRepository(SampleDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        // Not required to implement anything. Add additional functionalities if required.
    }
}
