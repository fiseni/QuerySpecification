using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore3
{
    public interface ISearchEvaluator<T> where T : class
    {
        Expression<Func<T, bool>>? GetExpression(string searchTerm, int searchType);
    }
}
