using Aggregates;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Validation
{
    public class ValidationException : BusinessException
    {
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base("Validation Failure")
        {
            Failures = failures;
        }

        public readonly IEnumerable<ValidationFailure> Failures;
    }
}
