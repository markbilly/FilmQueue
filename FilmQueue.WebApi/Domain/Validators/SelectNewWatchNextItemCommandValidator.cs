using FilmQueue.WebApi.DataAccess;
using FilmQueue.WebApi.Domain.Commands;
using FilmQueue.WebApi.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmQueue.WebApi.Domain.Validators
{
    public class SelectNewWatchNextItemCommandValidator : IValidator<SelectNewWatchNextItemCommand>
    {
        private readonly IWatchlistItemReader _watchlistItemReader;

        public SelectNewWatchNextItemCommandValidator(IWatchlistItemReader watchlistItemReader)
        {
            _watchlistItemReader = watchlistItemReader;
        }

        public async Task Validate(ValidationContext<SelectNewWatchNextItemCommand> validationContext)
        {
            var current = await _watchlistItemReader.GetCurrentWatchNextItem(validationContext.ValidationTarget.UserId);
            if (current != null)
            {
                validationContext.ValidationMessages.Add("state", "You already have a watch next item. Mark it as watched or skipped first.");
                return;
            }

            var unwatchedItemCount = await _watchlistItemReader.GetUnwatchedItemCount(validationContext.ValidationTarget.UserId);
            if (unwatchedItemCount == 0)
            {
                validationContext.ValidationMessages.Add("state", "You have no more unwatched items. Cannot select a new watch next item.");
                return;
            }
        }
    }
}
