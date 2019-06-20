using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Validators
{
    public class UpdateWatchlistItemCommandValidator : IValidator<UpdateWatchlistItemCommand>
    {
        private readonly IWatchlistItemReader _watchlistItemReader;

        public UpdateWatchlistItemCommandValidator(IWatchlistItemReader watchlistItemReader)
        {
            _watchlistItemReader = watchlistItemReader;
        }

        public async Task Validate(ValidationContext<UpdateWatchlistItemCommand> validationContext)
        {
            var item = await _watchlistItemReader.GetItemById(validationContext.ValidationTarget.ItemId);
            if (item == null)
            {
                validationContext.ValidationMessages.Add("notfound", "Watchlist item does not exist.");
                return;
            }

            if (string.IsNullOrWhiteSpace(validationContext.ValidationTarget.Title))
            {
                validationContext.ValidationMessages.Add("title", "Title is a required field.");
                return;
            }

            if (validationContext.ValidationTarget.Title.Length > 200)
            {
                validationContext.ValidationMessages.Add("title", "Title cannot be longer than 200 characters.");
            }

            if (validationContext.ValidationTarget.RuntimeInMinutes <= 0)
            {
                validationContext.ValidationMessages.Add("runtimeInMinutes", "Runtime is not valid. Must be positive whole number.");
            }
        }
    }
}
