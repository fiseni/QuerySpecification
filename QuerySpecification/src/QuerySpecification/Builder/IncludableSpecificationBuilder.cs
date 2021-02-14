﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public class IncludableSpecificationBuilder<T, TProperty> : IIncludableSpecificationBuilder<T, TProperty>
    {
        public Specification<T> Specification { get; }

        public IncludableSpecificationBuilder(Specification<T> specification)
        {
            Specification = specification;
        }
    }
}
