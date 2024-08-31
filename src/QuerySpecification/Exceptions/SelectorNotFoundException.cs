using System;
using System.Collections.Generic;
using System.Text;

namespace PozitronDev.QuerySpecification
{
    public class SelectorNotFoundException : Exception
    {
        private const string message = "The specification must have Selector defined.";

        public SelectorNotFoundException()
            : base(message)
        {
        }

        public SelectorNotFoundException(Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
