using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public class OrderedSpecificationBuilder<T> : IOrderedSpecificationBuilder<T> where T : class
    {
        public Specification<T> Specification { get; }

        public OrderedSpecificationBuilder(Specification<T> specification)
        {
            this.Specification = specification;
        }


    }
}
