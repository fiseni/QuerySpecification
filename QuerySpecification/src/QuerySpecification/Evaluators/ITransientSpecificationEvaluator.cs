using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public interface ITransientSpecificationEvaluator
    {
        List<TResult> Evaluate<T, TResult>(IEnumerable<T> source, ISpecification<T, TResult> specification) where T : class;
        List<T> Evaluate<T>(IEnumerable<T> source, ISpecification<T> specification) where T : class;
    }
}
