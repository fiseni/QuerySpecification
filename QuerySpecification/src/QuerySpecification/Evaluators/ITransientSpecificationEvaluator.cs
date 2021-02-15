using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public interface ITransientSpecificationEvaluator
    {
        IEnumerable<TResult> Evaluate<T, TResult>(IEnumerable<T> source, ISpecification<T, TResult> specification) where T : class;
        IEnumerable<T> Evaluate<T>(IEnumerable<T> source, ISpecification<T> specification) where T : class;
    }
}
