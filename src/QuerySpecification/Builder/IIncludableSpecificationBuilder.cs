using System;
using System.Collections.Generic;
using System.Text;

namespace Pozitron.QuerySpecification
{
    public interface IIncludableSpecificationBuilder<T, out TProperty> : ISpecificationBuilder<T> where T : class
    {
    }
}
