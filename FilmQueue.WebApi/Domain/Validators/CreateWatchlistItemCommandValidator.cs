using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Validators
{
    public class CreateWatchlistItemCommandValidator : IValidator<CreateWatchlistItemCommand>
    {
        public Task Validate(ValidationContext<CreateWatchlistItemCommand> validationContext)
        {
            // TODO: check that the user exists

            if (string.IsNullOrWhiteSpace(validationContext.ValidationTarget.Title))
            {
                validationContext.ValidationMessages.Add("title", "Title is a required field");
            }

            return Task.CompletedTask;
        }
    }
}
