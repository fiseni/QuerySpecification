﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PozitronDev.QuerySpecification.EntityFrameworkCore
{
    public class AsNoTrackingWithIdentityResolutionEvaluator : IEvaluator
    {
        private AsNoTrackingWithIdentityResolutionEvaluator() { }
        public static AsNoTrackingWithIdentityResolutionEvaluator Instance { get; } = new AsNoTrackingWithIdentityResolutionEvaluator();

        public bool IsCriteriaEvaluator { get; } = true;

        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
        {
            if (specification.AsNoTrackingWithIdentityResolution)
            {
                // No support in EF Core 3
                // query = query.AsNoTrackingWithIdentityResolution();
            }

            return query;
        }
    }
}
