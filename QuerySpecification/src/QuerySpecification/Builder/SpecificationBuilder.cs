using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public class SpecificationBuilder<T, TResult> : SpecificationBuilder<T>, ISpecificationBuilder<T, TResult> where T : class
    {
        public new Specification<T, TResult> Specification { get; }

        public SpecificationBuilder(Specification<T, TResult> specification) 
            : base(specification)
        {
            this.Specification = specification;
        }
    }

    public class SpecificationBuilder<T> : ISpecificationBuilder<T> where T : class
    {
        public Specification<T> Specification { get; }

        public SpecificationBuilder(Specification<T> specification)
        {
            this.Specification = specification;
        }
    }
}
