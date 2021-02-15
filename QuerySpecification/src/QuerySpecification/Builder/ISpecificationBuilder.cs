using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public interface ISpecificationBuilder<T, TResult> : ISpecificationBuilder<T> where T : class
    {
        new Specification<T, TResult> Specification { get; }
    }

    public interface ISpecificationBuilder<T> where T : class
    {
        Specification<T> Specification { get; }
    }
}
