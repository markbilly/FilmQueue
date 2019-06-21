using FilmQueue.WebApi.Infrastructure.Events;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.Validation
{
    public class ValidationFailedEvent<T> : IEvent where T : class
    {
        public ValidationFailedEvent(ValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }

        public ValidationResult ValidationResult { get; }
    }
}
