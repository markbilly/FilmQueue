using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.FluentValidation
{
    public static class ValidationResultExtensions
    {
        public static bool IsNotFoundResult(this ValidationResult validationResult)
        {
            return validationResult.Errors.Any(error => error.ErrorCode == "404");
        }
    }
}
