using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public interface IIncludableSpecificationBuilder<T, out TProperty>
    {
        IIncludeAggregator Aggregator { get; }
    }
}
