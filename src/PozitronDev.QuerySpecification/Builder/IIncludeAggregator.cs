using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification.Builder
{
    public interface IIncludeAggregator
    {
        void AddNavigationPropertyName(string navigationPropertyName);
        string IncludeString { get; }
    }
}
