using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Infrastructure.FluentValidation
{
    public static class CustomValidatorOptions
    {
        public static IRuleBuilderOptions<T, TProperty> ResourceNotFoundRule<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule)
        {
            return rule.WithErrorCode("404");
        }
    }
}
