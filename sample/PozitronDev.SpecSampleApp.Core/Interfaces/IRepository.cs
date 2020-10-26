using PozitronDev.QuerySpecification;
using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.SpecSampleApp.Core.Interfaces
{
    public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
    {
    }
}
